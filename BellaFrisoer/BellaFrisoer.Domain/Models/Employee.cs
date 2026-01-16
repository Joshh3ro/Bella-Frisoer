using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }
    public long PhoneNumber { get; set; }
    public string? Email { get; set; }
    public decimal HourlyPrice { get; set; }
    public string Qualifications { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
    public Employee() { }
}