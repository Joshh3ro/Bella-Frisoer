using System;
using System.Collections.Generic;
using System.Linq;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application
{
    public class BookingConflictChecker : IBookingConflictChecker
    {
        public bool HasDateTimeConflict(DateTime newStartTime, TimeSpan newDuration, DateTime existingStartTime, TimeSpan existingDuration)
        {
            DateTime newEndTime = newStartTime.Add(newDuration);
            DateTime existingEndTime = existingStartTime.Add(existingDuration);
            return newStartTime < existingEndTime && existingStartTime < newEndTime;
        }

        public bool HasBookingConflict(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            foreach (var booking in existingBookings)
            {
                bool sameCustomer = booking.CustomerId == newBooking.CustomerId;
                bool sameEmployee = booking.EmployeeId == newBooking.EmployeeId;

                if (!(sameCustomer || sameEmployee))
                    continue;

                // Normalize both start times to the booking date + time-of-day to avoid mismatches in Date component.
                var newStart = newBooking.BookingDate.Date + newBooking.BookingStartTime.TimeOfDay;
                var existingStart = booking.BookingDate.Date + booking.BookingStartTime.TimeOfDay;

                // Be robust: ignore bookings missing a valid duration
                if (newBooking.BookingDuration <= TimeSpan.Zero || booking.BookingDuration <= TimeSpan.Zero)
                    continue;

                if (HasDateTimeConflict(newStart, newBooking.BookingDuration, existingStart, booking.BookingDuration))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
