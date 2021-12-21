using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetFirstActivitySpec : Specification<Activity>, ISingleResultSpecification
{
    public GetFirstActivitySpec()
    {
        Query.OrderBy(x => x.HikeDate);
    }
}