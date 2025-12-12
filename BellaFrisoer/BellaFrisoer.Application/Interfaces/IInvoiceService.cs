using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<string> GenerateInvoiceAsync(int bookingId);
    }
}
