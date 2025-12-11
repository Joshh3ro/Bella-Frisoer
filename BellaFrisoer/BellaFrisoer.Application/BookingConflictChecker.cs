using System;
using System.Collections.Generic;
using System.Linq;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class BookingConflictChecker : IBookingConflictChecker
    {
        public bool HasBookingConflict(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            foreach (var booking in existingBookings)
            {
                bool sameEmployee = booking.EmployeeId == newBooking.EmployeeId;

                if (!sameEmployee)
                    continue;

                var newStart = newBooking.CombineDateTime(newBooking.BookingDate.Date, newBooking.BookingStartTime);
                var newEnd = newStart.Add(newBooking.BookingDuration);

                var existingStart = newBooking.CombineDateTime(booking.BookingDate.Date, booking.BookingStartTime);
                var existingEnd = existingStart.Add(booking.BookingDuration);

                if (newBooking.BookingDuration <= TimeSpan.Zero || booking.BookingDuration <= TimeSpan.Zero)
                    continue;

                if (newStart < existingEnd && existingStart < newEnd)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
