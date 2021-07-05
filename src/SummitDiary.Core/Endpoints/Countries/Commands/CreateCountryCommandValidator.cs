using FluentValidation;

namespace SummitDiary.Core.Endpoints.Countries.Commands
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .NotEmpty();
        }
    }
}