using BellaFrisoer.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Services
{
    public class BookingService
    {
        public decimal CalculateFinalPrice(decimal price, IDiscountStrategy strategy)
        {
            var discount = new Discount(strategy);
            return discount.Apply(price);
        }
    }
}
