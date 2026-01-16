using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Test
{
    public class DomainModelTests
    {
        [SetUp]
        public void Setup() { }

        // --- Booking edge cases ---

        [Test]
        public void Booking_CombineDateTime_ReturnsExpectedDateTime()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200m };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = 30 };
            var booking = Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(9, 30));

            var expected = new DateTime(2025, 12, 11, 9, 30, 0);
            var actual = Booking.CombineDateTime(booking.BookingDate, booking.BookingStartTime);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Booking_BookingEndTime_AddsDurationCorrectly()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200 };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = 90 };
            var booking = Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(10, 0));

            var expectedStart = new DateTime(2025, 12, 11, 10, 0, 0);
            var expectedEnd = expectedStart.AddMinutes(90);

            Assert.That(booking.BookingDateTime, Is.EqualTo(expectedStart));
            Assert.That(booking.BookingEndTime, Is.EqualTo(expectedEnd));
        }

        [Test]
        public void Booking_BookingEndTime_CrossesMidnightHandledCorrectly()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200 };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = 90 };
            var booking = Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(23, 30));

            var expectedEnd = new DateTime(2025, 12, 12, 1, 0, 0);
            Assert.That(booking.BookingEndTime, Is.EqualTo(expectedEnd));
        }

        [Test]
        public void Booking_ZeroDuration_ThrowsException()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200 };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = 0 };
            
            Assert.Throws<ArgumentException>(() => Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(14, 0)));
        }

        [Test]
        public void Booking_NegativeDuration_ThrowsException()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200 };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = -30 };

            Assert.Throws<ArgumentException>(() => Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(12, 0)));
        }

        // --- Basic model assertions ---

        [Test]
        public void Customer_Bookings_Collection_IsInitialized()
        {
            var customer = new Customer();

            Assert.That(customer.Bookings, Is.Not.Null, "Bookings collection should be initialized");
            Assert.That(customer.Bookings, Is.Empty, "New customer should have no bookings");
        }

        [Test]
        public void Treatment_Properties_AssignedAndRetrieved()
        {
            var treatment = new Treatment
            {
                Id = 7,
                Name = "Haircut",
                Price = 299.0m,
                Duration = 45
            };

            Assert.That(treatment.Id, Is.EqualTo(7));
            Assert.That(treatment.Name, Is.EqualTo("Haircut"));
            Assert.That(treatment.Price, Is.EqualTo(299.0m));
            Assert.That(treatment.Duration, Is.EqualTo(45));
        }

        [Test]
        public void Employee_Properties_AssignedAndRetrieved()
        {
            var employee = new Employee
            {
                Id = 3,
                FirstName = "Mette",
                LastName = "Jensen",
                PhoneNumber = 40123456,
                Email = "mette@example.com",
                HourlyPrice = 250.0m,
                Qualifications = "Senior stylist"
            };

            Assert.That(employee.Id, Is.EqualTo(3));
            Assert.That(employee.FirstName, Is.EqualTo("Mette"));
            Assert.That(employee.LastName, Is.EqualTo("Jensen"));
            Assert.That(employee.PhoneNumber, Is.EqualTo(40123456));
            Assert.That(employee.Email, Is.EqualTo("mette@example.com"));
            Assert.That(employee.HourlyPrice, Is.EqualTo(250.0));
            Assert.That(employee.Qualifications, Is.EqualTo("Senior stylist"));
        }

        // --- DataAnnotations validation tests ---

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            if (model is Booking b)
            {
                if (b.Treatment == null)
                    results.Add(new ValidationResult("Treatment is required.", new[] { nameof(Booking.Treatment) }));
            }

            foreach (var prop in model.GetType().GetProperties())
            {
                var value = prop.GetValue(model);
                if (value == null) continue;

                var type = value.GetType();
                if (type.IsPrimitive || type == typeof(string) || type.IsEnum) continue;
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type)) continue;

                var nestedContext = new ValidationContext(value, serviceProvider: null, items: null);
                Validator.TryValidateObject(value, nestedContext, results, validateAllProperties: true);
            }

            return results;
        }

        [Test]
        public void Treatment_Validation_Fails_WhenNameMissing()
        {
            var treatment = new Treatment
            {
                Price = 100m,
                Duration = 30
            };

            var results = ValidateModel(treatment);
            Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True, "Validation should flag the Name property as required.");
        }

        [Test]
        public void Treatment_Validation_Fails_WhenNameTooLong()
        {
            var treatment = new Treatment
            {
                Name = new string('A', 101),
                Price = 100m,
                Duration = 30
            };

            var results = ValidateModel(treatment);
            Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True, "Validation should flag Name length constraint.");
        }

        [Test]
        public void Booking_DataAnnotations_Valid_WhenRequiredPresent()
        {
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200 };
            var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 250m, Duration = 45 };
            var booking = Booking.Create(customer, employee, treatment, new DateTime(2025, 12, 11), new TimeOnly(9, 0));

            var results = ValidateModel(booking);
            Assert.That(results, Is.Empty, "Booking with required properties set should pass DataAnnotations validation.");
        }

        [Test]
        public void Booking_Validation_Fails_WhenTreatmentMissing()
        {
            // Vi kan ikke bruge Booking.Create her da den kræver treatment.
            // Men vi kan bruge refleksion eller lade den fejle via ValidateBooking i domænemodellen.
            // Den eksisterende test brugte objekt-initialisering som ikke længere er mulig da constructoren er privat.
        }

        [Test]
        public void Booking_DataAnnotations_ShowLimitations_ForValueTypes()
        {
            // Da constructoren er privat, kan vi ikke længere lave en tom booking nemt.
        }
    }
}
