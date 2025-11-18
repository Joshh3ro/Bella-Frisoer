using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

public class Employee : IPerson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public ICollection<String>? Treatments { get; set; } = new List<String>();
    public long? PhoneNumber { get; set; }

    public Employee() { }
}