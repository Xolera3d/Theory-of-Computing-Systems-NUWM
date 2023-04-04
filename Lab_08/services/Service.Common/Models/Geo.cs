using System.Text.Json.Serialization;

namespace Service.Common.Models;

public class FieldNearby
{
    [JsonPropertyName("point_of_interest")]
    public GeoPoint PointOfInterest { get; set; }

    [JsonPropertyName("field_data")] public GeoField Field { get; set; }
}

public class GeoField : GeoPoint
{
    [JsonPropertyName("name")] public string Name { get; set; }
}

public class GeoPoint
{
    public GeoPoint()
    { }

    public GeoPoint(double lat, double lon)
    {
        Lat = lat;
        Lon = lon;
    }
    [JsonPropertyName("lat")] public double Lat { get; set; }
    [JsonPropertyName("lon")] public double Lon { get; set; }
}