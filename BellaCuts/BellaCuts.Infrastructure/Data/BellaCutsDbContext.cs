using BellaCuts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BellaCuts.Infrastructure.Data;

public class BellaCutsDbContext : DbContext
{
    public BellaCutsDbContext(DbContextOptions<BellaCutsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Apply EF configurations if you add IEntityTypeConfiguration<> classes
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(BellaCutsDbContext).Assembly);
    }
}
