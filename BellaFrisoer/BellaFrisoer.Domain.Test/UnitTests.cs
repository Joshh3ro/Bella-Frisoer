using System;
using NUnit.Framework;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Test
{
    [TestFixture]
    public class BookingOverlapTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Overlap_ReturnsTrue_ForSameEmployee_WithOverlappingIntervals()
        {
            // Arrange — create navigation entities
            var customer1 = new Customer { Id = 1, FirstName = "Alice", LastName = "A", PhoneNumber = 40000001 };
            var employee = new Employee { Id = 10, FirstName = "Bob", LastName = "B", PhoneNumber = 40000002, HourlyPrice = 200m, Qualifications = "X" };
            var treatment1 = new Treatment { Id = 100, Name = "Cut", Price = 100m, Duration = 60 };
            var treatment2 = new Treatment { Id = 101, Name = "Wash", Price = 50m, Duration = 30 };

            // booking A: 10:00 - 11:00
            var bookingA = Booking.CreateBookingForUpdatePrice(customer1, employee, treatment1, new DateTime(2025, 1, 1), new TimeOnly(10, 0));

            // booking B: 10:30 - 11:00 (overlaps A)
            var bookingB = Booking.CreateBookingForUpdatePrice(
                new Customer { Id = 2, FirstName = "C", LastName = "D", PhoneNumber = 40000003 },
                employee,
                treatment2,
                new DateTime(2025, 1, 1),
                new TimeOnly(10, 30));

            // Act
            var aConflictsB = bookingA.ConflictsWith(bookingB);
            var bConflictsA = bookingB.ConflictsWith(bookingA);

            // Assert
            Assert.That(aConflictsB, Is.True, "Expected bookingA to conflict with bookingB (same employee, overlapping).");
            Assert.That(bConflictsA, Is.True, "Expected bookingB to conflict with bookingA (symmetric).");
        }

        [Test]
        public void Overlap_ReturnsFalse_ForDifferentEmployees_EvenIfTimesOverlap()
        {
            var customer1 = new Customer { Id = 1, FirstName = "Alice", LastName = "A", PhoneNumber = 40000001 };
            var employee1 = new Employee { Id = 10, FirstName = "Bob", LastName = "B", PhoneNumber = 40000002, HourlyPrice = 200m, Qualifications = "X" };
            var employee2 = new Employee { Id = 11, FirstName = "Eve", LastName = "E", PhoneNumber = 40000004, HourlyPrice = 180m, Qualifications = "Y" };
            var treatment = new Treatment { Id = 100, Name = "Cut", Price = 100m, Duration = 60 };

            var date = new DateTime(2025, 2, 1);

            // employee1: 10:00–11:00
            var bookingA = Booking.CreateBookingForUpdatePrice(customer1, employee1, treatment, date, new TimeOnly(10, 0));

            // employee2: 10:30–11:30 (overlaps in time)
            var bookingB = Booking.CreateBookingForUpdatePrice(
                new Customer { Id = 2, FirstName = "C", LastName = "D", PhoneNumber = 40000003 },
                employee2,
                treatment,
                date,
                new TimeOnly(10, 30));

            Assert.That(bookingA.ConflictsWith(bookingB), Is.False);
            Assert.That(bookingB.ConflictsWith(bookingA), Is.False);
        }
    }

        public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
