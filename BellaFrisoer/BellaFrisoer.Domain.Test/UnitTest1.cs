using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Test
{
    public class BookingTests
    {
        [Test]
        public void BookingCreation_ValidData_CreatesBooking()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200m };
            var treatment = new Treatment { Id = 1, Duration = 60, Price = 100m };

            var booking = Booking.Create(customer, employee, treatment,
                new DateTime(2025, 6, 15), new TimeOnly(10, 0));

            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.Customer.Id, Is.EqualTo(1));
            Assert.That(booking.Employee.Id, Is.EqualTo(1));
            Assert.That(booking.Treatment.Id, Is.EqualTo(1));
        }

    }
}
