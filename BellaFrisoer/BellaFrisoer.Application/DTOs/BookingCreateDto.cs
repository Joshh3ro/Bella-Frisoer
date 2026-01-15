namespace BellaFrisoer.Application.DTOs;
/// <summary>
/// Opretter lightweight DTO der opvarer IDer og Booking Data.
/// </summary>
public record BookingCreateDto
{
    public int CustomerId { get; init; }
    public int EmployeeId { get; init; }
    public int TreatmentId { get; init; }
    public DateTime BookingDate { get; init; }
    public TimeOnly BookingStartTime { get; init; }
}