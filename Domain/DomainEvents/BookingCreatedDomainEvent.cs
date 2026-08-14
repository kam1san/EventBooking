namespace Domain.DomainEvents
{
    public record BookingCreatedDomainEvent(Guid BookingId, Guid EventId, string UserId, int Seats, DateTime CreatedAt);
}
