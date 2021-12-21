using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetActivitiesInDateRangeSpec : Specification<Activity>
{
    public GetActivitiesInDateRangeSpec(DateTime lower, DateTime upper)
    {
        Query.Where(x => x.HikeDate >= lower && x.HikeDate < upper);
    }
}