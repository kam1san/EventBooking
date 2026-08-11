using MediatR;

namespace Application.Events.Commands.BookSeats
{
    public record BookSeatsCommand(Guid idEvent, string userId, int seats) : IRequest<Guid>;
}