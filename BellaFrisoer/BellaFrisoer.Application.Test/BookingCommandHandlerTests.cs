using Moq;
using BellaFrisoer.Application.CommandHandlers;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Test
{
    [TestFixture]
    public class BookingCommandHandlerTests
    {
        private Mock<IBookingService> _bookingServiceMock;
        private BookingCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _handler = new BookingCommandHandler(_bookingServiceMock.Object);
        }

        [Test]
        public async Task CreateAsync_CallsAddBookingAsyncOnService()
        {
            // Arrange
            var dto = new BookingCreateDto { CustomerId = 1, EmployeeId = 1, TreatmentId = 1 };

            // Act
            await _handler.CreateAsync(dto);

            // Assert
            _bookingServiceMock.Verify(s => s.AddBookingAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsUpdateBookingAsyncOnService()
        {
            // Arrange
            var dto = new BookingUpdateDto { Id = 1, CustomerId = 1, EmployeeId = 1, TreatmentId = 1 };

            // Act
            await _handler.UpdateAsync(dto);

            // Assert
            _bookingServiceMock.Verify(s => s.UpdateBookingAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenBookingExists_CallsDeleteBookingAsyncOnService()
        {
            // Arrange
            int bookingId = 1;
            var customer = new Customer { Id = 1 };
            var employee = new Employee { Id = 1, HourlyPrice = 200m };
            var treatment = new Treatment { Id = 1, Price = 100, Duration = 30 };
            
            // Vi bruger refleksion eller lignende da Booking constructoren er privat, men BookingService returnerer domæneobjekter.
            // For denne test kan vi mocke GetByIdAsync til at returnere et objekt.
            // Siden vi ikke kan nemt lave en Booking her uden en public factory eller constructor,
            // og Booking.Create returnerer en ny, kan vi bruge den.
            var booking = Booking.Create(customer, employee, treatment, DateTime.Now, new TimeOnly(10, 0));
            
            _bookingServiceMock.Setup(s => s.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            // Act
            await _handler.DeleteAsync(bookingId);

            // Assert
            _bookingServiceMock.Verify(s => s.DeleteBookingAsync(booking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenBookingDoesNotExist_DoesNotCallDeleteOnService()
        {
            // Arrange
            int bookingId = 1;
            _bookingServiceMock.Setup(s => s.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            // Act
            await _handler.DeleteAsync(bookingId);

            // Assert
            _bookingServiceMock.Verify(s => s.DeleteBookingAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
