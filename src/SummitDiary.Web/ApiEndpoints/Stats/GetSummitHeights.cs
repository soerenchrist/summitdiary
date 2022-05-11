using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetSummitHeights : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<BaseStatDto>>
{
    private readonly IReadRepository<Summit> _summitRepository;

    public GetSummitHeights(IReadRepository<Summit> summitRepository)
    {
        _summitRepository = summitRepository;
    }
    
    [HttpGet("/api/stats/summits/heights")]
    public override async Task<ActionResult<List<BaseStatDto>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        const int start = 2000;
        const int steps = 250;
        var climbedSummits = await _summitRepository.ListAsync(new GetSummitsWithDiaryEntriesSpec(), cancellationToken);

        if (!climbedSummits.Any())
            return new List<BaseStatDto>();
            
        var results = new List<BaseStatDto>();
        var highest = climbedSummits.Max(x => x.Height);
        for (int currentLower = start; currentLower < highest; currentLower += steps)
        {
            var summitCount = climbedSummits.Count(x => x.Height >= currentLower && x.Height < currentLower + steps);
            results.Add(new BaseStatDto
            {
                Name = $"{currentLower} - {currentLower+steps} m",
                Value = summitCount
            });
        }

        return results;
    }
}