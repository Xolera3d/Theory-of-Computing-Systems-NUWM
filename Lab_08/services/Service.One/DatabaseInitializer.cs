using System.Text.Json;
using KafkaFlow.Producers;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Service.Common;
using Service.Common.Models;

namespace Service.One;

public static class DatabaseInitializer
{
    public static async void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var producerAccessor = scope.ServiceProvider.GetRequiredService<IProducerAccessor>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        await dbContext.Database.EnsureCreatedAsync();

        var streamReader = File.OpenRead("./data/field_centroids.geojson");
        var features = JsonSerializer.Deserialize<GeoJson>(streamReader, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })
            ?.Features.Select(ToFeature) ?? throw new Exception("field_centroids.geojson empty");

        if (!await dbContext.Fields.AnyAsync())
        {
            await dbContext.Fields.AddRangeAsync(features);
            await dbContext.SaveChangesAsync();
            
            logger.LogInformation("~ Database seeded ~");
            await producerAccessor.GetProducer("main").ProduceAsync(string.Empty, "true");
            logger.LogInformation("~ Message produced ~");
        }
    }


    private static Field ToFeature(JFeature jFeature)
    {
        var feature = new Field
        {
            Index = jFeature.Properties.Id,
            Name = jFeature.Properties.Name,
        };

        var coordinates = jFeature.Geometry.Coordinates;

        feature.Point = new Point(coordinates[0], coordinates[1]);

        return feature;
    }
}