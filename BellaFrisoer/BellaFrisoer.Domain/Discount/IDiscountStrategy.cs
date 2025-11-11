using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Discounts
{
    public interface IDiscountStrategy
    {
        decimal Apply(decimal amount);
    }
}
