using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Rental> Rentals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
