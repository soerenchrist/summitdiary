using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetActivityByIdSpec : Specification<Activity>, ISingleResultSpecification
{
    public GetActivityByIdSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Attachments)
            .Include(x => x.Summits);
    }
}