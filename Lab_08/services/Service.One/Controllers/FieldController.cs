using KafkaFlow.Producers;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Service.Common;
using Service.Common.Models;

namespace Service.One.Controllers;

[ApiController]
[Route("[controller]")]
public class FieldController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IProducerAccessor _producerAccessor;
    private readonly ILogger<FieldController> _logger;

    public FieldController(AppDbContext dbContext, IProducerAccessor producerAccessor, ILogger<FieldController> logger)
    {
        _dbContext = dbContext;
        _producerAccessor = producerAccessor;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> AddAsync(FieldRequest fieldRequest)
    {
        var field = new Field
        {
            Index = fieldRequest.Id,
            Name = fieldRequest.Name,
            Point = new Point(fieldRequest.Point.Lon, fieldRequest.Point.Lat)
        };

        _dbContext.Add(field);
        await _dbContext.SaveChangesAsync();

        await _producerAccessor.GetProducer("main").ProduceAsync(string.Empty, "true");
        
        _logger.LogInformation("Produced");
        return Ok();
    }

    public record FieldRequest(int Id, string Name, GeoPoint Point);
}