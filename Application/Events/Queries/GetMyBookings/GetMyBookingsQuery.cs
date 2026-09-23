using Application.DTOs;
using MediatR;

namespace Application.Events.Queries.GetMyBookings
{
    public record GetMyBookingsQuery(string idUser) : IRequest<List<MyBookingDto>>;
}
