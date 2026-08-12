using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Events.Commands.BookSeats
{
    public class BookSeatsCommandHandler : IRequestHandler<BookSeatsCommand, Guid>
    {
        readonly IEventRepository eventRepository;
        public BookSeatsCommandHandler(IEventRepository eventRepository) 
        {
            this.eventRepository = eventRepository;
        }

        public async Task<Guid> Handle(BookSeatsCommand request, CancellationToken cancellationToken)
        {
            var ev = await eventRepository.GetByIdAsync(request.idEvent) ?? throw new NotFoundException(nameof(Event), request.idEvent);
            var booking = ev.BookSeats(request.userId, request.seats);
            await eventRepository.AddBookingAsync(booking);
            await eventRepository.SaveChangesAsync();

            return booking.Id;
        }
    }
}
