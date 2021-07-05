using FluentValidation;

namespace SummitDiary.Core.Endpoints.Summits.Commands
{
    public class CreateSummitCommandValidator : AbstractValidator<CreateSummitCommand>
    {
        public CreateSummitCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);
            RuleFor(x => x.Height)
                .GreaterThan(0);
            RuleFor(x => x.CountryId)
                .NotEqual(0);
            RuleFor(x => x.RegionId)
                .NotEqual(0);
        }
    }
}