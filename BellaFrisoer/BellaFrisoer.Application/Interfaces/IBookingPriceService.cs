using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingPriceService
    {
        Task<decimal> CalculateFinalPrice(UpdatePriceDto dto, Customer customer, Employee employee, Treatment treatment);

    }
}