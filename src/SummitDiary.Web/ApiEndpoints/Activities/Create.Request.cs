using FluentValidation;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class CreateActivityRequest
{
    public string Title { get; set; } = string.Empty;
    public DateTime HikeDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int Rating { get; set; }
    public double ElevationUp { get; set; }
    public double ElevationDown { get; set; }
    public double Distance { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int Duration { get; set; }
    public List<int> SummitIds { get; set; } = new();
}

public class CreateActivityRequestValidator : AbstractValidator<CreateActivityRequest>
{
    public CreateActivityRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.HikeDate)
            .LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5);
        RuleFor(x => x.ElevationUp)
            .GreaterThan(0);
        RuleFor(x => x.ElevationDown)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.Distance)
            .GreaterThan(0);
    }
}