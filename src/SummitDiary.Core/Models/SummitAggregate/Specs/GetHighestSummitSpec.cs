using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetHighestSummitSpec : Specification<Summit>, ISingleResultSpecification
{
    public GetHighestSummitSpec()
    {
        Query
            .Include(x => x.Region)
            .Include(x => x.Country)
            .Include(x => x.DiaryEntries)
            .Where(x => x.DiaryEntries!.Any())
            .OrderByDescending(x => x.Height);
    }
}