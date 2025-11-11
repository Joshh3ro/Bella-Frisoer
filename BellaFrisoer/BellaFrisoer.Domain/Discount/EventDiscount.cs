using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Discounts
{
    public class EventDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal amount)
        {
            return amount * 0.9m; // 10% discount
        }
    }
}
