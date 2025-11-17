using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.WebUi.Data;

public class BellaFrisoerWebUiContext : DbContext
{
    public BellaFrisoerWebUiContext(DbContextOptions<BellaFrisoerWebUiContext> options)
        : base(options)
    {
<<<<<<< Updated upstream
        public BellaFrisoerWebUiContext (DbContextOptions<BellaFrisoerWebUiContext> options)
            : base(options)
        {
        }

        public DbSet<BellaFrisoer.Domain.Models.Booking> Booking { get; set; } = default!;
=======
    }

    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Bookings)
            .WithOne(b => b.Customer!)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
>>>>>>> Stashed changes
    }
}
