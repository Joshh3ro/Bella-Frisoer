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

                // Normalize both start times to the booking date + time-of-day to avoid mismatches in Date component.
                var newStart = newBooking.BookingDate.Date + newBooking.BookingStartTime.TimeOfDay;
                var newEnd = newBooking.BookingEndTime;

                var existingStart = booking.BookingDate.Date + booking.BookingStartTime.TimeOfDay;
                var existingEnd = booking.BookingEndTime;

                // Be robust: ignore bookings missing a valid duration
                if (newBooking.BookingDuration <= TimeSpan.Zero || booking.BookingDuration <= TimeSpan.Zero)
                    continue;

                if (newStart < existingEnd && existingStart < newEnd)
                {
                    return true; // There is a conflict
                }

            }
            return false;
        }
    }
}
