using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetActivitiesWithSummitsAndCountriesSpec : Specification<Activity>
{
    public GetActivitiesWithSummitsAndCountriesSpec()
    {
        Query.Include(x => x.Summits)!
            .ThenInclude(x => x.Country);
    }
}