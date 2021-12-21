using Ardalis.Specification;

namespace SummitDiary.Core.Models.ActivityAggregate.Specs;

public sealed class GetActivitiesPaginatedSpec : Specification<Activity>
{
    public GetActivitiesPaginatedSpec(int pageSize, int pageNumber, 
        string sortBy = "hikeDate",
        bool sortDescending = true,
        string? searchText = null,
        int? summitId = null)
    {
        var builder = (sortBy, sortDescending) switch
        {
            ("title", true) => Query.OrderByDescending(x => x.Title),
            ("title", false) => Query.OrderBy(x => x.Title),
            ("hikeDate", true) => Query.OrderByDescending(x => x.HikeDate),
            ("hikeDate", false) => Query.OrderBy(x => x.HikeDate),
            ("elevationUp", true) => Query.OrderByDescending(x => x.ElevationUp),
            ("elevationUp", false) => Query.OrderBy(x => x.ElevationUp),
            ("elevationDown", true) => Query.OrderByDescending(x => x.ElevationDown),
            ("elevationDown", false) => Query.OrderBy(x => x.ElevationDown),
            ("distance", true) => Query.OrderByDescending(x => x.Distance),
            ("distance", false) => Query.OrderBy(x => x.Distance),
            ("duration", true) => Query.OrderByDescending(x => x.Duration),
            _ => Query.OrderBy(x => x.Duration),
        };
        builder.Include(x => x.Summits);
        
        if (!string.IsNullOrWhiteSpace(searchText))
            builder.Where(x => x.Title.ToLower().Contains(searchText));

        if (summitId != null)
        {
            builder.Where(x => x.Summits!.Any(y => y.Id == summitId.Value));
        }

        builder.Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
    }
}