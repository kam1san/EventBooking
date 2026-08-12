using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Events.Queries.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
    {
        readonly IEventRepository eventRepository;

        public GetAllEventsQueryHandler(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await eventRepository.GetAllAsync();

            return events.Select(ev => new EventDto(
                ev.Id,
                ev.Title,
                ev.Description,
                ev.Date,
                ev.TotalSeats,
                ev.AvailableSeats
            )).ToList();
        }
    }
}
