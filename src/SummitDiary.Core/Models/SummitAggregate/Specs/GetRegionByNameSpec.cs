using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetRegionByNameSpec : Specification<Region>, ISingleResultSpecification
{
    public GetRegionByNameSpec(string regionName)
    {
        Query.Where(x => x.Name == regionName);
    }
}