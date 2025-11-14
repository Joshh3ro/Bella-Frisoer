using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.WebUi.Data
{
    public class BellaFrisoerWebUiContext : DbContext
    {
        public BellaFrisoerWebUiContext(DbContextOptions<BellaFrisoerWebUiContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure consistent table names
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Customer>().ToTable("Customer");

        }
    }
}