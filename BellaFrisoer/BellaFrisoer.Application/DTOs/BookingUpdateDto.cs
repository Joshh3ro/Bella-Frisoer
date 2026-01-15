namespace BellaFrisoer.Application.DTOs;
/// <summary>
/// Opretter lightweight DTO der opdatere IDer og Booking Data.
/// </summary>
public record BookingUpdateDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public int EmployeeId { get; init; }
    public int TreatmentId { get; init; }
    public DateTime BookingDate { get; init; }
    public TimeOnly BookingStartTime { get; init; }
}