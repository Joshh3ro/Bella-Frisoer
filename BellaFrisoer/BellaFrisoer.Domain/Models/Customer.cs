namespace BellaFrisoer.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public long? CustomerPhoneNumber { get; set; }
    public DateTime? CustomerBirthDate { get; set; }

    public Customer(DateTime bookingDateTime, string customerName, string customerEmail, string customerAddress, int customerPhoneNumber, DateTime? customerBirthDate )
    {
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerAddress = customerAddress;
        CustomerPhoneNumber = customerPhoneNumber;
        CustomerBirthDate = customerBirthDate;

    }
}