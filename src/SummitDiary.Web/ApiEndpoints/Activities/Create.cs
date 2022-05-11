using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Web.Util;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class Create : EndpointBaseAsync
    .WithRequest<CreateActivityRequest>
    .WithActionResult<ActivityDto>
{
    private readonly IRepository<Activity> _activityRepository;
    private readonly IMapper _mapper;
    private readonly IReadRepository<Summit> _summitRepository;

    public Create(IRepository<Activity> activityRepository,
        IMapper mapper,
        IReadRepository<Summit> summitRepository)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
        _summitRepository = summitRepository;
    }
    
    [HttpPost("/api/activities")]
    [SwaggerOperation(
        Summary = "Create an activity",
        Description = "Create an activity",
        OperationId = "Activity.Create",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult<ActivityDto>> HandleAsync(CreateActivityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        DateTime? startTime = null;
        DateTime? endTime = null;
        request.HikeDate = request.HikeDate.Date;
        if (request.StartTime != null && request.EndTime != null)
        {
            startTime = request.StartTime.ParseTime(request.HikeDate);
            endTime = request.EndTime.ParseTime(request.HikeDate);

            if (startTime != null && endTime != null)
            {

                if (endTime < startTime)
                    (startTime, endTime) = (endTime, startTime);

                request.Duration = (int) (endTime - startTime).Value.TotalSeconds;
            }
        }

        var summits = await GetSummits(request.SummitIds, cancellationToken);
        if (summits == null)
            return NotFound();
        var activity = new Activity
        {
            Title = request.Title,
            Distance = request.Distance,
            Duration = request.Duration,
            Notes = request.Notes,
            Rating = request.Rating,
            ElevationDown = request.ElevationDown,
            ElevationUp = request.ElevationUp,
            EndTime = endTime,
            StartTime = startTime,
            HikeDate = request.HikeDate,
            Summits = summits
        };

        await _activityRepository.AddAsync(activity, cancellationToken);
        return _mapper.Map<ActivityDto>(activity);
    }

    private async Task<List<Summit>?> GetSummits(List<int> summitIDs, CancellationToken cancellationToken)
    {
        var summits = new List<Summit>();
        foreach (var summitId in summitIDs)
        {
            var summit = await _summitRepository.GetByIdAsync(summitId, cancellationToken);
            if (summit == null)
                return null;
                
            summits.Add(summit);
        }

        return summits;
    }
}