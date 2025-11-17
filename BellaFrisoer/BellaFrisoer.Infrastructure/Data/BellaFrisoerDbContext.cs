using BellaFrisoer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Infrastructure.Data
{
    public class BellaFrisoerDbContext : DbContext
    {
        public BellaFrisoerDbContext(DbContextOptions<BellaFrisoerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Add other entities here later...
    }
}
