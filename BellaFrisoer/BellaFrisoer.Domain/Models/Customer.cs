using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

public class Customer : IPerson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    public long? PhoneNumber { get; set; }

    public Customer() { }
}