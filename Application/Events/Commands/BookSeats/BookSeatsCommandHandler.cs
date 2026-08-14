using Application.Interfaces;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Commands.BookSeats
{
    public class BookSeatsCommandHandler : IRequestHandler<BookSeatsCommand, Guid>
    {
        readonly IEventRepository eventRepository;
        readonly IMessagePublisher messagePublisher;
        public BookSeatsCommandHandler(IEventRepository eventRepository, IMessagePublisher messagePublisher) 
        {
            this.eventRepository = eventRepository;
            this.messagePublisher = messagePublisher;
        }

        public async Task<Guid> Handle(BookSeatsCommand request, CancellationToken cancellationToken)
        {
            var ev = await eventRepository.GetByIdAsync(request.idEvent) ?? throw new NotFoundException(nameof(Event), request.idEvent);
            var booking = ev.BookSeats(request.userId, request.seats);
            await eventRepository.AddBookingAsync(booking);
            await eventRepository.SaveChangesAsync();

            await messagePublisher.PublishAsync<BookingCreatedDomainEvent>(new BookingCreatedDomainEvent(booking.Id, booking.EventId, booking.UserId, booking.SeatsBooked, booking.CreatedAt), cancellationToken);

            return booking.Id;
        }
    }
}
