using Application.DTOs;
using MediatR;

namespace Application.Events.Queries.GetAllEvents
{
    public record GetAllEventsQuery : IRequest<List<EventDto>>;
}
