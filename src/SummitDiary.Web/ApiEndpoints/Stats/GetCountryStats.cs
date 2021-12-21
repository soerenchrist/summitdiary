using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetCountryStats : BaseAsyncEndpoint
    .WithRequest<GetCountryStatsRequest>
    .WithResponse<List<BaseStatDto>>
{
    private readonly IReadRepository<Activity> _activityRepository;

    public GetCountryStats(IReadRepository<Activity> activityRepository)
    {
        _activityRepository = activityRepository;
    }
    
    [HttpGet("/api/stats/country")]
    public override async Task<ActionResult<List<BaseStatDto>>> HandleAsync([FromQuery] GetCountryStatsRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var activities = await _activityRepository.ListAsync(new GetActivitiesWithSummitsAndCountriesSpec(), cancellationToken);
        Func<Activity, double> selector = request.ValueType switch
        {
            "distance" => activity => activity.Distance,
            _ => activity => activity.ElevationUp
        };
        
        var results = new List<BaseStatDto>();
        var countries = activities
            .SelectMany(x => x.Summits!)
            .Select(x => x.Country!)
            .Distinct();
        
        foreach (var country in countries)
        {
            var value = activities.Where(x => 
                    x.Summits!.Any(s => s.CountryId == country.Id))
                .Sum(selector);
            results.Add(new BaseStatDto
            {
                Name = country.Name,
                Value = value
            });
        }

        return results;
    }
}