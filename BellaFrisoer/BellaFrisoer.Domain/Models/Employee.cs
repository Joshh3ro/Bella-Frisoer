using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Models;

public class Employee
{
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public string? Email { get; set; }
        public double HourlyPrice { get; set; }

        public List<Treatment> Qualifications { get; set; } = new();

        [NotMapped]
        public int SelectedTreatmentId { get; set; }


    public Employee() { }
}




