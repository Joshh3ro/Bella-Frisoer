using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Contracts.Queries;

namespace BellaFrisoer.Infrastructure.Quieries
{
    public class BookingQuery : IBookingQuery
    {
        private readonly BellaFrisoerWebUiContext _dbContext;

        public BookingQuery(BellaFrisoerWebUiContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<List<BookingDto>> IBookingQuery.GetAllAsync()
        {
            return await _dbContext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    Customer = b.Customer != null ? new CustomerDto { Id = b.Customer.Id, FirstName = b.Customer.FirstName, LastName = b.Customer.LastName } : null,
                    Employee = b.Employee != null ? new EmployeeDto { Id = b.Employee.Id, FirstName = b.Employee.FirstName, LastName = b.Employee.LastName } : null,
                    Treatment = b.Treatment != null ? new TreatmentDto { Id = b.Treatment.Id, Name = b.Treatment.Name } : null,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var b = await _dbContext.Bookings
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Treatment)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b == null) return null;

            return new BookingDto
            {
                Id = b.Id,
                Customer = b.Customer != null ? new CustomerDto { Id = b.Customer.Id, FirstName = b.Customer.FirstName, LastName = b.Customer.LastName } : null,
                Employee = b.Employee != null ? new EmployeeDto { Id = b.Employee.Id, FirstName = b.Employee.FirstName, LastName = b.Employee.LastName } : null,
                Treatment = b.Treatment != null ? new TreatmentDto { Id = b.Treatment.Id, Name = b.Treatment.Name } : null,
                BookingDate = b.BookingDate
            };
        }

        public async Task<List<BookingDto>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date)
        {
            return await _dbContext.Bookings
                .Where(b => b.Employee.Id == employeeId && b.BookingDate.Date == date.Date)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    Customer = b.Customer != null ? new CustomerDto { Id = b.Customer.Id, FirstName = b.Customer.FirstName, LastName = b.Customer.LastName } : null,
                    Employee = b.Employee != null ? new EmployeeDto { Id = b.Employee.Id, FirstName = b.Employee.FirstName, LastName = b.Employee.LastName } : null,
                    Treatment = b.Treatment != null ? new TreatmentDto { Id = b.Treatment.Id, Name = b.Treatment.Name } : null,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();
        }

        public async Task<List<BookingDto>> FilterBookingsAsync(string searchTerm)
        {
            return await _dbContext.Bookings
                .Where(b =>
                    (b.Customer != null && b.Customer.FirstName.Contains(searchTerm)) ||
                    (b.Customer != null && b.Customer.LastName.Contains(searchTerm)) ||
                    (b.Employee != null && b.Employee.FirstName.Contains(searchTerm)) ||
                    (b.Employee != null && b.Employee.LastName.Contains(searchTerm)) ||
                    (b.Treatment != null && b.Treatment.Name.Contains(searchTerm)))
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    Customer = b.Customer != null ? new CustomerDto { Id = b.Customer.Id, FirstName = b.Customer.FirstName, LastName = b.Customer.LastName } : null,
                    Employee = b.Employee != null ? new EmployeeDto { Id = b.Employee.Id, FirstName = b.Employee.FirstName, LastName = b.Employee.LastName } : null,
                    Treatment = b.Treatment != null ? new TreatmentDto { Id = b.Treatment.Id, Name = b.Treatment.Name } : null,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();
        }
    }
}
