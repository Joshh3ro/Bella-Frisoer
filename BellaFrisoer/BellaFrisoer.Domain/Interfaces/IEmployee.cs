using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Interfaces
{
    public interface IEmployee :IPerson
    {
        public ICollection<String>? Treatments { get; set; }
        public double HourlyPrice { get; set; }
    }
}