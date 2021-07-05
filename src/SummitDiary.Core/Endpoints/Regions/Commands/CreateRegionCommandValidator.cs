using FluentValidation;
using SummitDiary.Core.Endpoints.Countries.Commands;

namespace SummitDiary.Core.Endpoints.Regions.Commands
{
    public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .NotEmpty();
        }
    }
}