using System;
using System.Collections.Generic;
using System.Linq;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application
{
    internal class BookingConflictChecker
    {
        public bool HasDateTimeConflict(DateTime newStartTime, TimeSpan newDuration, DateTime existingStartTime, TimeSpan existingDuration)
        {
            DateTime newEndTime = newStartTime.Add(newDuration);
            DateTime existingEndTime = existingStartTime.Add(existingDuration);
            return newStartTime < existingEndTime && existingStartTime < newEndTime;
        }

        public bool HasBookingConflict(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            //foreach (var booking in existingBookings)
            //{
            //    bool sameCustomer = booking.CustomerId == newBooking.CustomerId;
            //    bool sameEmployee = booking.EmployeeId == newBooking.EmployeeId;

            //    if ((sameCustomer || sameEmployee) &&
            //        HasDateTimeConflict(newBooking.StartTime, newBooking.Duration, booking.StartTime, booking.Duration))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
    }
}
