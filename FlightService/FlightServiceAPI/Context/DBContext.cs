using Microsoft.EntityFrameworkCore;

namespace FlightServiceAPI.Context
{
    public class DBContext : DbContext
    {
        public DbSet<AIRLINE_FLIGHTS> AIRLINE_FLIGHTS { get; set; }
        public DbSet<AIRLINE_AIRPORTS> AIRLINE_AIRPORTS { get; set; }

        public DbSet<AIRLINE_TICKETS> AIRLINE_TICKETS { get; set; }
        public DbSet<AIRLINE_ADMINS> AIRLINE_ADMINS { get; set; }

        public DbSet<AIRLINE_MILESMEMBERS> AIRLINE_MILESMEMBERS { get; set; }
        public DbSet<AIRLINE_CITIES> AIRLINE_CITIES { get; set; }


        public IConfiguration _configuration;
        public DBContext(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AIRLINE_AIRPORTS>()
                .HasKey(a => a.id);

            modelBuilder.Entity<AIRLINE_FLIGHTS>()
                .HasKey(f => f.id);

            modelBuilder.Entity<AIRLINE_FLIGHTS>(e =>
            {
                e.HasOne(f => f.departPort)
                  .WithMany(dp => dp.DepartureFlights)
                  .HasForeignKey(f => f.departure_airport);

                e.HasOne(f => f.arrivePort)
                  .WithMany(ap => ap.ArrivalFlights)
                  .HasForeignKey(f => f.arrival_airport);          
            });       
        }
    }
}
