using Application.DTOs;
using MediatR;

namespace Application.Events.Queries.GetEventById
{
    public record GetEventByIdQuery(Guid idEvent) : IRequest<EventDto>;
}
