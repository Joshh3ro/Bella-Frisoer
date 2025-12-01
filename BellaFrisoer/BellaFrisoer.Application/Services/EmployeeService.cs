using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IBookingRepository _repository;
        private readonly IBookingConflictChecker _conflictChecker;

        public EmployeeService(IBookingRepository repository, IBookingConflictChecker conflictChecker)
        {
            _repository = repository;
            _conflictChecker = conflictChecker;
        }

        public async Task<bool> CanCreateBookingAsync(Booking newBooking)
        {
            var all = await _repository.GetAllAsync();
            var relevant = all.Where(b => b.EmployeeId == newBooking.EmployeeId).ToList();
            return !_conflictChecker.HasBookingConflict(newBooking, relevant);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _repository.AddAsync(booking);
        }

        public async Task<List<Booking>> GetAllAsync() => await _repository.GetAllAsync();
    }
}