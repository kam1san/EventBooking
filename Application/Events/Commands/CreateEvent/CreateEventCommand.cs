using MediatR;

namespace Application.Events.Commands.CreateEvent
{
    public record CreateEventCommand(string title, string description, DateTime date, int totalSeats) : IRequest<Guid>;
}
