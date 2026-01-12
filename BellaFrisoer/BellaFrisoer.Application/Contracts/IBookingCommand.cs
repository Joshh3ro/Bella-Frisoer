using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts
{
    public interface IBookingCommand
    {
        Task CreateAsync
        Task AddCustomerToBookingAsync()
    }

    public class AddCustomerToBookingCommandDto
    {

    }
}
