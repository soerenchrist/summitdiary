using Ardalis.Specification;
using SummitDiary.Core.Models.Common;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetSummitsPaginatedSpec : Specification<Summit>
{
    public GetSummitsPaginatedSpec(int pageSize, int pageNumber,
        string sortBy = "name",
        bool sortDescending = false,
        string? searchText = null,
        bool onlyClimbed = false,
        Bounds? bounds = null)
    {
        SetupBuilder(pageSize, pageNumber, sortBy, sortDescending, searchText, onlyClimbed, bounds);
    }
    
    public GetSummitsPaginatedSpec(string sortBy = "name",
        bool sortDescending = false,
        string? searchText = null,
        bool onlyClimbed = false,
        Bounds? bounds = null)
    {
        SetupBuilder(null, null, sortBy, sortDescending, searchText, onlyClimbed, bounds);
    }

    private void SetupBuilder(int? pageSize = null,
        int? pageNumber = null,
        string sortBy = "name",
        bool sortDescending = false,
        string? searchText = null,
        bool onlyClimbed = false,
        Bounds? bounds = null)
    {
        var builder = (sortBy, sortDescending) switch
        {
            ("height", true) => Query.OrderByDescending(x => x.Height),
            ("name", true) => Query.OrderByDescending(x => x.Name),
            ("height", false) => Query.OrderBy(x => x.Height),
            _ => Query.OrderBy(x => x.Name)
        };
        builder.Include(x => x.DiaryEntries)
            .Include(x => x.Country)
            .Include(x => x.Region);
        
        if (!string.IsNullOrWhiteSpace(searchText))
            builder.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));

        if (onlyClimbed)
            builder.Where(x => x.DiaryEntries!.Any());

        if (bounds != null)
        {
            builder.Where(x => x.Latitude >= bounds.SwLat 
                               && x.Longitude >= bounds.SwLon
                               && x.Latitude <= bounds.NeLat &&
                               x.Longitude <= bounds.NeLon);
        }

        if (pageNumber != null && pageSize != null)
        {
            builder.Skip(pageSize.Value * (pageNumber.Value - 1))
                .Take(pageSize.Value);
        }
    }
}