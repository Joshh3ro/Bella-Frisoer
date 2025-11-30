using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

public class Customer 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }
    public long PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }

    public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();

    public Customer() { }
}