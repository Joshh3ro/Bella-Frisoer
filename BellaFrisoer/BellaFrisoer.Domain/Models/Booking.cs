using System.ComponentModel.DataAnnotations;



namespace BellaFrisoer.Domain.Models;



public class Booking

{

    //[Key] // Unødvendigt, da EF konventioner antager at "Id" er PK

    public int Id { get; protected set; }



    public DateTime BookingDateTime => CombineDateTime(BookingDate, BookingStartTime);



    [Required] public DateTime BookingDate { get; protected set; }



    public TimeOnly BookingStartTime { get; protected set; }



    public TimeSpan BookingDuration { get; protected set; }



    public DateTime BookingEndTime => CombineDateTime(BookingDate, BookingStartTime).Add(BookingDuration);



    public decimal TotalPrice { get; protected set; }



    //// scalar FK: what EF uses & what forms bind to

    //[Required]

    //public int CustomerId { get; protected set; } // Unødvenditg, da vi har navigation property



    public Customer Customer { get; protected set; } // Not null !!  - ? slettet



    //[Required]

    //public int EmployeeId { get; protected set; } // Unødvendigt, da vi har navigation property



    //[ForeignKey(nameof(EmployeeId))] // Unødvendigt, pga. implicit Entity Framework konvention

    public Employee Employee { get; protected set; } // Not null !!  - ? slettet





    // [Required]

    //public int TreatmentId { get; protected set; } // Unødvendigt, da vi har navigation property

    //[ForeignKey(nameof(TreatmentId))]

    public Treatment Treatment { get; protected set; }

    //[Timestamp]

    //public byte[] RowVersion { get; protected set; } // Unødvendigt, da vi ikke bruger concurrency kontrol - det er et enkelt bruger system



    private Booking() // for EF

    {

    }



    //private Booking(..................) // TODO: Tilføj parametre

    //{

    //    Forretningslogin der skal være opfyldt for at oprette en booking

    //   

    //}



    //public static Booking Create(........) // Factory metode. TODO: Tilføj parametre

    //{

    //    return new Booking(............);

    //}



    public DateTime CombineDateTime(DateTime date, TimeOnly time)

    {

        return date.Date.Add(time.ToTimeSpan());

    }

}