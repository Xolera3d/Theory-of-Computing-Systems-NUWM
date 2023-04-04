using System.Text.Json;
using System.Text.Json.Serialization;
using GeoTiffCOG;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Service.Common.Models;

namespace Service.Three;

public class FieldProcessingHandler : IMessageHandler<FieldNearby>
{
    private readonly ILogger<FieldProcessingHandler> _logger;
    private readonly GeoTiff _geoTiff;

    public FieldProcessingHandler(ILogger<FieldProcessingHandler> logger, GeoTiff geoTiff)
    {
        _logger = logger;
        _geoTiff = geoTiff;
    }

    public async Task Handle(IMessageContext context, FieldNearby message)
    {
        var field = message.Field;
        var moisture = _geoTiff.GetElevationAtLatLon(field.Lat, field.Lon);

        var r = new Result
        {
            Field = field,
            PointOfInterest = message.PointOfInterest,
            Moisture = moisture
        };

        var str = JsonSerializer.Serialize(r, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync($"./results/{r.PointOfInterest.Lat} {r.PointOfInterest.Lon}.json", str);

        _logger.LogInformation("The data was written to a file");
    }
}

public class Result : FieldNearby
{
    [JsonPropertyName("moisture")] public float Moisture { get; set; }
}