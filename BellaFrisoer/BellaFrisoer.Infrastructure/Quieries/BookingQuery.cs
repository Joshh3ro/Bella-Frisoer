using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Infrastructure.Quieries
{
    public class BookingQuery
    {
        private readonly BellaFrisoerWebUiContext _dbContext;

        public BookingQuery (BellaFrisoerWebUiContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
