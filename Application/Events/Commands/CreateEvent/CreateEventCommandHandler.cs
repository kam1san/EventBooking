using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
        readonly IEventRepository eventRepository;

        public CreateEventCommandHandler(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var ev = new Event(request.title, request.description, request.date, request.totalSeats);
            await eventRepository.Add(ev);
            await eventRepository.SaveChanges();

            return ev.Id;
        }
    }
}
