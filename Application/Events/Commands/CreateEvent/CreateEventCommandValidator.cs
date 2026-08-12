using FluentValidation;

namespace Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.description).MaximumLength(2000);
            RuleFor(x => x.date).GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future");
            RuleFor(x => x.totalSeats).GreaterThan(0).LessThanOrEqualTo(100000);
        }
    }
}
