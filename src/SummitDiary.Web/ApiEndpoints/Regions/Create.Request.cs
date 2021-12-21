using FluentValidation;

namespace SummitDiary.Web.ApiEndpoints.Regions;

public class CreateRegionRequest
{
    public string Name { get; set; } = String.Empty;
}

public class CreateRegionCommandValidator : AbstractValidator<CreateRegionRequest>
{
    public CreateRegionCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}