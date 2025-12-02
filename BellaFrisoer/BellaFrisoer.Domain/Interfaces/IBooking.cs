using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Interfaces
{
    internal interface IBooking
    {
        int Id { get; set; }
        DateTime BookingDateTime { get; }

        DateTime BookingDate { get; set; }

        DateTime BookingStartTime { get; set; }

        TimeSpan BookingDuration { get; set; }

        DateTime BookingEndTime { get; }

        int CustomerId { get; set; }

        Customer? Customer { get; set; }

        int EmployeeId { get; set; }

        Employee? Employee { get; set; }

        DateTime CombineDateTime(DateTime date, TimeSpan time);
    }


}

