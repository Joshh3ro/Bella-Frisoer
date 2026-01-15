using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Queries
{
    public interface IBookingQuery
    {
        Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken);
        Task<Booking> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Booking>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date, CancellationToken cancellationToken);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken);
    }

}