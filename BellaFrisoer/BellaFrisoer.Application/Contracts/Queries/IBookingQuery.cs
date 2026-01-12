using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts.Queries
{
    public interface IBookingQuery
    {
        Task<List<BookingDto>> GetAllAsync();
        Task<BookingDto> GetByIdAsync(int id);
        Task<List<BookingDto>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date);
        Task<List<BookingDto>> FilterBookingsAsync(string searchTerm);
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
