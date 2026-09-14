namespace Application.DTOs
{
    public record BookingDto(Guid Id, Guid EventId, int SeatsBooked, DateTime CreatedAt);
}
