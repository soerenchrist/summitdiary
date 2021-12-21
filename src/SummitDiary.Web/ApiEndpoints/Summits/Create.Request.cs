using FluentValidation;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class CreateSummitRequest
{
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CountryId { get; set; }
    public int RegionId { get; set; }
}


public class CreateSummitCommandValidator : AbstractValidator<CreateSummitRequest>
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