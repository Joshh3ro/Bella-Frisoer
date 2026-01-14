using System;
using BellaFrisoer.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
        Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        //Load metode
    }
}
