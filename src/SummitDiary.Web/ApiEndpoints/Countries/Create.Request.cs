using FluentValidation;

namespace SummitDiary.Web.ApiEndpoints.Countries;

public class CreateCountryRequest
{
    public string Name { get; set; } = string.Empty;
}

public class CreateCountryCommandValidator : AbstractValidator<CreateCountryRequest>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}