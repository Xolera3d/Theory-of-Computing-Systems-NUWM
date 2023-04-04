using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Service.Common;

public class AppDbContext : DbContext
{
    private readonly string _connection;
    
    public AppDbContext(string connection) => _connection = connection;

    public AppDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Field> Fields { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(
                _connection,
                o => o.UseNetTopologySuite());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("postgis");

        // https://stackoverflow.com/questions/61452283/ef-core-spatial-data-query-and-get-distances-in-meters
        builder.Entity<Field>()
            .Property(e => e.Point)
            .HasColumnType("geography (point)");
    }
}

public class Field
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; }
    public Point Point { get; set; }
}