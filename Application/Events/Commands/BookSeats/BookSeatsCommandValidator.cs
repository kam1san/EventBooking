using FluentValidation;

namespace Application.Events.Commands.BookSeats
{
    public class BookSeatsCommandValidator : AbstractValidator<BookSeatsCommand>
    {
        public BookSeatsCommandValidator()
        {
            RuleFor(x => x.idEvent).NotEmpty();
            RuleFor(x => x.userId).NotEmpty();
            RuleFor(x => x.seats).GreaterThan(0);
        }
    }
}
