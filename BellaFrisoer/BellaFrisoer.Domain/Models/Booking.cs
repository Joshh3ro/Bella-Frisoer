using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Models.Discounts;



namespace BellaFrisoer.Domain.Models;



public class Booking

{

    //[Key] // Unødvendigt, da EF konventioner antager at "Id" er PK

    public int Id { get; private set; }



    // Booking date (date component). Stored as DateTime for compatibility with existing migrations.

    [Required] public DateTime BookingDate { get; private set; }



    [Required] public TimeOnly BookingStartTime { get; private set; }



    [Required] public TimeSpan BookingDuration { get; private set; }



    // Computed values (not mapped) for convenience

    [NotMapped] public DateTime BookingStartDateTime => CombineDateTime(BookingDate, BookingStartTime);



    [NotMapped] public DateTime BookingEndDateTime => BookingStartDateTime + BookingDuration;



    [Required] [Column(TypeName = "decimal(18,2)")] public decimal TotalPrice { get; private set; }


    // Navigation properties (required for domain invariants)

    [Required] public Customer Customer { get; private set; } = null!;



    [Required] public Employee Employee { get; private set; } = null!;



    [Required] public Treatment Treatment { get; private set; } = null!;



    [Timestamp] public byte[] RowVersion { get; private set; } = null!;



    // Parameterless ctor for EF

    private Booking() { }



    /// <summary>

    /// Factory method to create a booking with domain validations applied.

    /// </summary>

    public static Booking Create(Customer customer, Employee employee, Treatment treatment, DateTime bookingDate, TimeOnly startTime, TimeSpan? duration = null)

    {

        if (customer is null) throw new ArgumentNullException(nameof(customer));

        if (employee is null) throw new ArgumentNullException(nameof(employee));

        if (treatment is null) throw new ArgumentNullException(nameof(treatment));



        var effectiveDuration = duration ?? TimeSpan.FromMinutes(treatment.Duration);

        if (effectiveDuration <= TimeSpan.Zero) throw new ArgumentException("Duration must be greater than zero.", nameof(duration));



        var booking = new Booking

        {

            Customer = customer,

            CustomerId = customer.Id,

            Employee = employee,

            EmployeeId = employee.Id,

            Treatment = treatment,

            TreatmentId = treatment.Id,

            BookingDate = bookingDate.Date,

            BookingStartTime = startTime,

            BookingDuration = effectiveDuration,

            TotalPrice = treatment.Price

        };



        // Any other business rules (e.g. employee qualifications vs treatment) should be enforced here

        return booking;

    }



    /// <summary>

    /// Reschedule booking date/time. Keeps duration and price unchanged.

    /// </summary>

    public void Reschedule(DateTime newDate, TimeOnly newStartTime)

    {

        BookingDate = newDate.Date;

        BookingStartTime = newStartTime;

    }



    /// <summary>

    /// Change the assigned employee for the booking.

    /// Validation (qualifications, availability) should be performed by higher-level domain/service if needed.

    /// </summary>

    public void AssignEmployee(Employee newEmployee)

    {

        if (newEmployee is null) throw new ArgumentNullException(nameof(newEmployee));

        Employee = newEmployee;

        EmployeeId = newEmployee.Id;

    }



    /// <summary>

    /// Change the customer for the booking.

    /// </summary>

    public void AssignCustomer(Customer newCustomer)

    {

        if (newCustomer is null) throw new ArgumentNullException(nameof(newCustomer));

        Customer = newCustomer;

        CustomerId = newCustomer.Id;

    }



    /// <summary>

    /// Change treatment. Updates duration and base price to match the treatment.

    /// </summary>

    public void ChangeTreatment(Treatment newTreatment)

    {

        if (newTreatment is null) throw new ArgumentNullException(nameof(newTreatment));

        Treatment = newTreatment;

        TreatmentId = newTreatment.Id;



        // Update dependent characteristics

        BookingDuration = TimeSpan.FromMinutes(newTreatment.Duration);

        TotalPrice = newTreatment.Price;

    }



    /// <summary>

    /// Apply a domain discount strategy to the booking's total price.

    /// The discount logic lives in domain-level strategies that implement IDiscountStrategy.

    /// </summary>

    public void ApplyDiscount(IDiscountStrategy discountStrategy)

    {

        if (discountStrategy is null) return;

        TotalPrice = discountStrategy.Apply(TotalPrice);

        if (TotalPrice < 0) TotalPrice = 0m;

    }



    /// <summary>

    /// Combine a date and a TimeOnly to a DateTime (helper).

    /// </summary>

    public DateTime CombineDateTime(DateTime date, TimeOnly time)

    {

        return date.Date + time.ToTimeSpan();

    }



    // -------------------------

    // Methods moved from BookingService (domain/OOP behavior)

    // -------------------------

    /// <summary>

    /// Update duration from a treatment. If treatment is null or invalid sets duration to zero.

    /// </summary>

    public void UpdateDurationFromTreatment(Treatment? treatment)

    {

        if (treatment == null || treatment.Id <= 0)

        {

            BookingDuration = TimeSpan.Zero;

            return;

        }



        BookingDuration = TimeSpan.FromMinutes(treatment.Duration);

    }



    /// <summary>

    /// Validate the booking's required fields. Returns tuple (IsValid, ErrorMessage).

    /// </summary>

    public (bool IsValid, string? ErrorMessage) Validate()

    {

        if (CustomerId <= 0)

            return (false, "Vælg kunde...");

        if (EmployeeId <= 0)

            return (false, "Vælg ansat...");

        if (TreatmentId <= 0)

            return (false, "Vælg behandling...");

        if (BookingStartTime == default)

            return (false, "Ugyldigt starttidspunkt.");

        if (BookingDuration == default || BookingDuration <= TimeSpan.Zero)

            return (false, "Ugyldig varighed.");

        return (true, null);

    }



    /// <summary>

    /// Determine whether this booking overlaps with another booking (simple interval overlap).

    /// </summary>

    public bool OverlapsWith(Booking other)

    {

        if (other is null) throw new ArgumentNullException(nameof(other));

        return BookingStartDateTime < other.BookingEndDateTime && other.BookingStartDateTime < BookingEndDateTime;

    }



    /// <summary>

    /// Determine discount strategy for a customer's total bookings.

    /// Kept as a domain helper that inspects the customer's bookings collection.

    /// </summary>

    public static IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer)

    {

        if (customer is null) return null;



        int count = customer.Bookings?.Count ?? 0;



        if (count >= 20)

            return new GoldDiscount();

        if (count >= 10)

            return new SilverDiscount();

        if (count >= 5)

            return new BronzeDiscount();



        return null;

    }



    /// <summary>

    /// Determine discount strategy for a customer's bookings for a specific treatment.

    /// </summary>

    public static IDiscountStrategy? GetDiscountStrategyForCustomerAndTreatment(Customer customer, int treatmentId)

    {

        if (customer is null) return null;

        if (treatmentId <= 0) return null;



        var countForTreatment = customer.Bookings?

            .Count(b => b.TreatmentId == treatmentId) ?? 0;



        if (countForTreatment >= 20)

            return new GoldDiscount();

        if (countForTreatment >= 10)

            return new SilverDiscount();

        if (countForTreatment >= 5)

            return new BronzeDiscount();



        return null;

    }

}