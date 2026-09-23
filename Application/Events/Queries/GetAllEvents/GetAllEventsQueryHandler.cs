using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Events.Queries.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
    {
        readonly IEventRepository eventRepository;
        readonly IMapper mapper;

        public GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await eventRepository.GetAllAsync();
            return mapper.Map<List<EventDto>>(events);
        }
    }
}