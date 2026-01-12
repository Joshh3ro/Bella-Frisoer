using BellaFrisoer.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts.Commands
{
    public class BookingCommand : IBookingCommand
    {
        public readonly IBookingRepository _repository;
        public BookingCommand(IBookingRepository repository) 
        {
            _repository = repository;
        }
        public Task CreateAsync(BookingCreateCommandDto dto)
        {
            throw new NotImplementedException();
        }
    }
    {
    }
}
