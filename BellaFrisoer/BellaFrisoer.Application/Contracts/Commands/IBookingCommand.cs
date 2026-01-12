using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts.Commands
{
    public interface IBookingCommand
    {
        Task CreateAsync(BookingCreateCommandDto dto);
    }

    public class BookingCreateCommandDto
    {
        public string Name { get; set; }
    }
}
