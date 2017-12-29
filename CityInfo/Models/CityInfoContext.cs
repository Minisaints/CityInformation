using Microsoft.EntityFrameworkCore;

namespace CityInfo.Models
{
    public sealed class CityInfoContext : DbContext
    {

        public CityInfoContext(DbContextOptions options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}
