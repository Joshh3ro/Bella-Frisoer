using System;
using System.Collections.Generic;
using BellaFrisoer.Application;
using BellaFrisoer.Domain.Models;
using NUnit.Framework;

namespace BellaFrisoer.Application.Test
{
    [TestFixture]
    public class BookingConflictCheckerTests
    {
        private Booking? _newBooking;
        private List<Booking>? _existingBookings;
        private BookingConflictChecker _checker;

        [SetUp]
        public void Setup()
        {
            _checker = new BookingConflictChecker();
        }

        [Test]
        public void HasBookingConflict_ReturnsTrue_WhenEmployeeHasOverlappingBooking()
        {
            // Arrange
            _existingBookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    CustomerId = 1,
                    EmployeeId = 1,
                    BookingDate = new DateTime(2024, 7, 1),
                    BookingStartTime = new DateTime(2024, 7, 1, 9, 0, 0),
                    BookingDuration = TimeSpan.FromHours(1)
                },
                // This booking spans 11:00 - 17:00 and uses EmployeeId = 1
                new Booking
                {
                    Id = 2,
                    CustomerId = 2,
                    EmployeeId = 1,
                    BookingDate = new DateTime(2024, 7, 1),
                    BookingStartTime = new DateTime(2024, 7, 1, 11, 0, 0),
                    BookingDuration = TimeSpan.FromHours(6)
                }
            };

            // New booking is at 13:00 for EmployeeId = 1 -> should conflict with booking Id = 2
            _newBooking = new Booking
            {
                Id = 3,
                CustomerId = 3,
                EmployeeId = 1,
                BookingDate = new DateTime(2024, 7, 1),
                BookingStartTime = new DateTime(2024, 7, 1, 13, 0, 0),
                BookingDuration = TimeSpan.FromHours(1)
            };

            // Act
            bool hasConflict = _checker.HasBookingConflict(_newBooking, _existingBookings!);

            // Assert (NUnit 4 uses the constraint model)
            Assert.That(hasConflict, Is.True, "Expected a conflict because the new booking overlaps an existing booking for the same employee.");
        }

        [Test]
        public void HasBookingConflict_ReturnsFalse_WhenDifferentEmployeeAndNoOverlap()
        {
            // Arrange: reuse the same existing bookings
            _existingBookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    CustomerId = 1,
                    EmployeeId = 1,
                    BookingDate = new DateTime(2024, 7, 1),
                    BookingStartTime = new DateTime(2024, 7, 1, 9, 0, 0),
                    BookingDuration = TimeSpan.FromHours(1)
                },
                new Booking
                {
                    Id = 2,
                    CustomerId = 2,
                    EmployeeId = 1,
                    BookingDate = new DateTime(2024, 7, 1),
                    BookingStartTime = new DateTime(2024, 7, 1, 11, 0, 0),
                    BookingDuration = TimeSpan.FromHours(6)
                }
            };

            // New booking is at 17:00 for EmployeeId = 2 -> no overlap and different employee
            _newBooking = new Booking
            {
                Id = 4,
                CustomerId = 4,
                EmployeeId = 2,
                BookingDate = new DateTime(2024, 7, 1),
                BookingStartTime = new DateTime(2024, 7, 1, 17, 0, 0),
                BookingDuration = TimeSpan.FromHours(1)
            };

            // Act
            bool hasConflict = _checker.HasBookingConflict(_newBooking, _existingBookings!);

            // Assert (NUnit 4 uses the constraint model)
            Assert.That(hasConflict, Is.False, "Expected no conflict because the new booking does not overlap and is for a different employee.");
        }

        [Test]
        public void HasBookingConflict_ReturnsTrue_WhenExistingBookingSpansMidnight_AndNewBookingOnNextDateOverlaps()
        {
            // Arrange
            // Existing booking starts late on July 1 and spans into July 2 (e.g., 22:00 -> 02:00)
            _existingBookings = new List<Booking>
            {
                new Booking
                {
                    Id = 20,
                    CustomerId = 20,
                    EmployeeId = 7,
                    BookingDate = new DateTime(2024, 7, 1),
                    BookingStartTime = new DateTime(2024, 7, 1, 22, 0, 0),
                    BookingDuration = TimeSpan.FromHours(4) // ends on July 2 at 02:00
                }
            };

            // New booking is on July 2 at 01:00 -> should overlap with the existing booking despite different BookingDate
            _newBooking = new Booking
            {
                Id = 21,
                CustomerId = 21,
                EmployeeId = 7,
                BookingDate = new DateTime(2024, 7, 2),
                BookingStartTime = new DateTime(2024, 7, 2, 1, 0, 0),
                BookingDuration = TimeSpan.FromHours(1) // 01:00 - 02:00
            };

            // Act
            bool hasConflict = _checker.HasBookingConflict(_newBooking, _existingBookings!);

            // Assert
            Assert.That(hasConflict, Is.True, "Expected a conflict because the existing booking spans midnight into the new booking's date and times overlap.");
        }
    }
}
