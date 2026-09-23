using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        readonly IEventRepository eventRepository;
        readonly IMapper mapper;

        public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var ev = await eventRepository.GetByIdAsync(request.idEvent) ?? throw new NotFoundException(nameof(Event), request.idEvent);
            return mapper.Map<EventDto>(ev);
        }
    }
}