using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Services
{
    public class DiscountCalculatorService
    {
        public async Task<DiscountResult> BeregnBedsteRabat(Customer kunde, Booking booking)
        {
            var result = new DiscountResult();
            var lockObj = new object();

            // Strategierne ligger i Domain-laget
            var strategier = new List<IDiscountStrategy>
            {
                new BronzeDiscount(),
                new SølvDiscount(),
                new GuldDiscount(),
                new EventDiscount()
            };

            // Parallel afvikling
            var tasks = strategier.Select(strategy => Task.Run(() =>
            {
                var rabat = strategy.Beregn(kunde, booking);

                lock (lockObj)
                {
                    result.OpdaterHvisBedre(rabat);
                }
            }));

            await Task.WhenAll(tasks);

            return result;
        }
    }
}
