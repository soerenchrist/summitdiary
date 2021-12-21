using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetActivitiesSpec : Specification<Activity>
{
    public GetActivitiesSpec()
    {
        Query.Include(x => x.Summits);
    }
}