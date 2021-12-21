using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetActivityTimeline : BaseAsyncEndpoint
    .WithRequest<GetActivityTimelineRequest>
    .WithResponse<List<TimelineStatDto>>
{
    private readonly IReadRepository<Activity> _activityRepository;

    public GetActivityTimeline(IReadRepository<Activity> activityRepository)
    {
        _activityRepository = activityRepository;
    }
    
    [HttpGet("/api/stats/timeline")]
    public override async Task<ActionResult<List<TimelineStatDto>>> HandleAsync([FromQuery] GetActivityTimelineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var count = await _activityRepository.CountAsync(cancellationToken);
        if (count == 0)
            return new List<TimelineStatDto>();

        var (currentDate, interval) = request.TimeType switch
        {
            "week" => (DateTime.Now.AddDays(-7), 7),
            "month" => (DateTime.Now.AddMonths(-1), 7),
            "all" => (
                (await _activityRepository.GetBySpecAsync(new GetFirstActivitySpec(), cancellationToken))!.HikeDate,
                30),
            _ => (DateTime.Now.AddYears(-1), 30)
        };
        
        
        Func<Activity, double> selector = request.ValueType switch
        {
            "distance" => activity => activity.Distance,
            _ => activity => activity.ElevationUp
        };
        
        var results = new List<TimelineStatDto>();
        while (currentDate <= DateTime.Now)
        {
            var nextDate = currentDate.AddDays(interval);

            var date = currentDate;
            var activities = await _activityRepository.ListAsync(new GetActivitiesInDateRangeSpec(date, nextDate),
                cancellationToken);
            
            results.Add(new TimelineStatDto
            {
                Date = currentDate,
                Value = (int) activities.Sum(selector)
            });
                
            currentDate = nextDate;
        }

        return results;
    }
}