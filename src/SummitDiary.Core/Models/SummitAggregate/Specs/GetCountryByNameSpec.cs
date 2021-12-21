using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetCountryByNameSpec : Specification<Country>, ISingleResultSpecification
{
    public GetCountryByNameSpec(string regionName)
    {
        Query.Where(x => x.Name == regionName);
    }
}