using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetSummitByIdSpec : Specification<Summit>, ISingleResultSpecification
{
    public GetSummitByIdSpec(int id)
    {
        Query
            .Include(x => x.Country)
            .Include(x => x.Region)
            .Include(x => x.OsmData)
            .Where(x => x.Id == id);
    }
}