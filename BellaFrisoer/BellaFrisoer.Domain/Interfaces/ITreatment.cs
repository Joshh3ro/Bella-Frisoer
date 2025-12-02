using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Interfaces
{
    public interface ITreatment
    {
        int Id { get; set; }
        string Name { get; set; }

        string ProductNumber { get; set; }

        int DurationMinutes { get; set; }

       
        decimal HourlyRate { get; set; }
    }
}
