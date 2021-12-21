using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetTotals : BaseAsyncEndpoint
    .WithoutRequest
    .WithResponse<TotalsDto>
{
    private readonly IReadRepository<Activity> _activityRepository;

    public GetTotals(IReadRepository<Activity> activityRepository)
    {
        _activityRepository = activityRepository;
    }
    
    [HttpGet("/api/stats/totals")]
    public override async Task<ActionResult<TotalsDto>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var distance = await _activityRepository.SumAsync(x => x.Distance, cancellationToken);
        var elevation = await _activityRepository.SumAsync(x => x.ElevationUp, cancellationToken);
        var duration = await _activityRepository.SumAsync(x => x.Duration, cancellationToken);
        var activityCount = await _activityRepository.CountAsync(cancellationToken);
        var activities = await _activityRepository.ListAsync(new GetActivitiesSpec(), cancellationToken);
        var summitCount = activities.SelectMany(x => x.Summits!).Distinct()
            .Count();
            
        return new TotalsDto
        {
            Distance = (int)distance,
            Elevation = (int)elevation,
            Duration = duration,
            ActivityCount = activityCount,
            SummitCount = summitCount
        };
    }
}