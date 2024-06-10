using ElectricityApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.EfStructures;

public class ElectricityDbContext : DbContext
{
    public DbSet<ElectricityConsumption> ElectricityConsumptions => Set<ElectricityConsumption>();

    public ElectricityDbContext(DbContextOptions<ElectricityDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ElectricityConsumption>()
            .Property(e => e.AmountToPay)
            .HasConversion<double>();
    }
}