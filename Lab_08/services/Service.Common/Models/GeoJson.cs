namespace Service.Common.Models
{
    public class GeoJson
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Crs Crs { get; set; }
        public List<JFeature> Features { get; set; }
    }

    public class Crs
    {
        public string Type { get; set; }
        public CrsProperties Properties { get; set; }
    }

    public class CrsProperties
    {
        public string Name { get; set; }
    }

    public class JFeature
    {
        public string Type { get; set; }
        public FeatureProperties Properties { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class FeatureProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}