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

    public class BookingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public EmployeeDto Employee { get; set; }

        public CustomerDto Customer { get; set; }

        public TreatmentDto Treatment { get; set; }

        public DateTime BookingDate { get; set; }
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class TreatmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}