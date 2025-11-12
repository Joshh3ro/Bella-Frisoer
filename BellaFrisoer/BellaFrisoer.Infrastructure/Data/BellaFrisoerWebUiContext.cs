using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.WebUi.Data
{
    public class BellaFrisoerWebUiContext : DbContext
    {
        public BellaFrisoerWebUiContext (DbContextOptions<BellaFrisoerWebUiContext> options)
            : base(options)
        {
        }

        public DbSet<BellaFrisoer.Domain.Models.Booking> Booking { get; set; } = default!;
    }
}
