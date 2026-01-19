namespace BellaFrisoer.Application.DTOs;
/// <summary>
///  Opretter vores delete objekt til booking command
/// </summary>
public class BookingDeleteDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public int EmployeeId { get; init; }
    public int TreatmentId { get; init; }
    public DateTime BookingDate { get; init; }
    public TimeOnly BookingStartTime { get; init; }
}