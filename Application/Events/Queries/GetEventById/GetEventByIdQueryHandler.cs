using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        readonly IEventRepository eventRepository;

        public GetEventByIdQueryHandler(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var ev = await eventRepository.GetByIdAsync(request.idEvent) ?? throw new NotFoundException(nameof(Event), request.idEvent);

            return new EventDto(
                ev.Id,
                ev.Title,
                ev.Description,
                ev.Date,
                ev.TotalSeats,
                ev.AvailableSeats
            );
        }
    }
}
