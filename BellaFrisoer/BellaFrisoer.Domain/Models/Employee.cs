using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

public class Employee : IEmployee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }
    public long PhoneNumber { get; set; }
    public string? Email { get; set; }
    public double HourlyPrice { get; set; }
    public ICollection<String>? Treatments { get; set; } = new List<String>();

    public Employee() { }
}