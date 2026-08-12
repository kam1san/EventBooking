using FluentValidation;

namespace Application.Events.Queries.GetEventById
{
    public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
    {
        public GetEventByIdQueryValidator()
        {
            RuleFor(x => x.idEvent).NotEmpty();
        }
    }
}
