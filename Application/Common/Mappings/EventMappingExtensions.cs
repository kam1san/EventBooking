using Application.DTOs;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public static class EventMappingExtensions
    {
        public static EventDto ToDto(this Event ev) => new EventDto(
            ev.Id,
            ev.Title,
            ev.Description,
            ev.Date,
            ev.TotalSeats,
            ev.AvailableSeats,
            ev.Bookings.Select(b => new BookingDto(b.Id, b.EventId, b.SeatsBooked, b.CreatedAt)).ToList()
        );
    }
}
