using System;
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
            .WithOne(b => b.Customers)
            .HasForeignKey(b => b.CustomerId) // Booking.CustomerId is the scalar FK
            .OnDelete(DeleteBehavior.Cascade); // when a Customer is deleted, their bookings are deleted

        // Employee (1) -> Booking (many)
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Bookings)       // ensure Employee class has ICollection<Booking> Bookings
            .WithOne(b => b.Employee)
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascading delete of bookings when deleting an employee

        // Optional: if Employee <-> Treatment is many-to-many, map the join table.
        // Uncomment and adjust if you have navigation collections on both sides:
        /*
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Treatments) // ICollection<Treatment> on Employee
            .WithMany(t => t.Employees) // ICollection<Employee> on Treatment
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeTreatment",
                j => j.HasOne<Treatment>().WithMany().HasForeignKey("TreatmentId"),
                j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"));
        */
    }
}