using System;
using FluentValidation;

namespace SummitDiary.Core.Endpoints.Diary.Commands
{
    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
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
            RuleFor(x => x.SummitIds)
                .NotEmpty();
        }
    }
}