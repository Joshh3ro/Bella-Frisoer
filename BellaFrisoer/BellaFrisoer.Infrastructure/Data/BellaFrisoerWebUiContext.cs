using BellaFrisoer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BellaFrisoer.Infrastructure.Data;

public class BellaFrisoerWebUiContext : DbContext
{
    public BellaFrisoerWebUiContext(DbContextOptions<BellaFrisoerWebUiContext> options)
        : base(options)
    {
    }

    // Use concrete entity types (not interfaces) for DbSet
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Treatment> Treatments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table mappings
        modelBuilder.Entity<Booking>().ToTable("Bookings");
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Treatment>().ToTable("Treatments");

        // Customer (1) -> Booking (many)
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Bookings)
            .WithOne(b => b.Customer)
            .HasForeignKey(b => b.CustomerId) // Booking.CustomerId is the scalar FK
            .OnDelete(DeleteBehavior.Cascade); // when a Customer is deleted, their bookings are deleted

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasPrecision(18, 2); // or use .HasColumnType("decimal(18,2)")

        modelBuilder.Entity<Treatment>()
            .Property(t => t.Price)
            .HasPrecision(18, 2); // or use .HasColumnType("decimal(18,2)")
    }
}