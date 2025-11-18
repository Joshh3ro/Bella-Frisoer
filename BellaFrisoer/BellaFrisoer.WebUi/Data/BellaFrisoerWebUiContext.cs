using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.WebUi.Data;

public class BellaFrisoerWebUiContext : DbContext
{
    public BellaFrisoerWebUiContext(DbContextOptions<BellaFrisoerWebUiContext> options)
        : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure table names / relationships are explicit and match migrations/snapshot
        modelBuilder.Entity<Booking>().ToTable("Bookings");
        modelBuilder.Entity<Customer>().ToTable("Customers");

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Bookings)
            .WithOne(b => b.Customer)
            .HasForeignKey(b => b.CustomerId) // use the scalar FK property
            .OnDelete(DeleteBehavior.Cascade);
    }
}
