using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.TypedHandler;
using NetTopologySuite.Geometries;
using Service.Common;
using Service.Common.Models;

namespace Service.Two;

public class FieldReadingHandler : IMessageHandler<string>
{
    private readonly IProducerAccessor _producer;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<FieldReadingHandler> _logger;

    public FieldReadingHandler(IProducerAccessor producer, AppDbContext dbContext, ILogger<FieldReadingHandler> logger)
    {
        _producer = producer;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, string message)
    {
        var points = (await File.ReadAllTextAsync("./data/POI.txt")).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x =>
            {
                var c = x.Split(',').Select(double.Parse).ToArray();
                return new Point(c.First(), c.Last());
            });

        foreach (var point in points)
        {
            var nearbyCity = _dbContext.Fields.OrderBy(x => x.Point.Distance(point)).First();

            var fieldNearby = new FieldNearby
            {
                PointOfInterest = new GeoPoint(point.Y, point.X),
                Field = new GeoField
                {
                    Name = nearbyCity.Name,
                    Lat = nearbyCity.Point.Y,
                    Lon = nearbyCity.Point.X,
                }
            };


            await _producer.GetProducer("main").ProduceAsync(string.Empty, fieldNearby);
            _logger.LogInformation("Produce field '{Field}'", fieldNearby.Field.Name);
        }
    }
}