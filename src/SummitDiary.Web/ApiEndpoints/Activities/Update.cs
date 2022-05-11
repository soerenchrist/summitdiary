using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;
using SummitDiary.Web.Util;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateActivityRequest>
    .WithActionResult<ActivityDto>
{
    private readonly IRepository<Activity> _activityRepository;
    private readonly IMapper _mapper;

    public Update(IRepository<Activity> activityRepository,
        IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }
    
    [HttpPut("/api/activities")]
    [SwaggerOperation(
        Summary = "Update activity",
        Description = "Update activity",
        OperationId = "Activity.Update",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult<ActivityDto>> HandleAsync(UpdateActivityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        DateTime? endTime = null;
        request.HikeDate = request.HikeDate.Date;
        if (request.StartTime != null && request.EndTime != null)
        {
            var startTime = request.StartTime.ParseTime(request.HikeDate);
            endTime = request.EndTime.ParseTime(request.HikeDate);

            if (startTime != null && endTime != null)
            {

                if (endTime < startTime)
                    (startTime, endTime) = (endTime, startTime);

                request.Duration = (int) (endTime - startTime).Value.TotalSeconds;
            }
        }
        
        var activity = await _activityRepository.GetBySpecAsync(new GetActivityByIdSpec(request.Id), cancellationToken);
        if (activity == null)
            return NotFound();
        
        activity.Distance = request.Distance;
        activity.Duration = request.Duration;
        activity.Notes = request.Notes;
        activity.Rating = request.Rating;
        activity.ElevationDown = request.ElevationDown;
        activity.ElevationUp = request.ElevationUp;
        activity.EndTime = endTime;
        activity.StartTime = endTime;
        activity.HikeDate = request.HikeDate;
        activity.Title = request.Title;

        await _activityRepository.UpdateAsync(activity, cancellationToken);
        return _mapper.Map<ActivityDto>(activity);
    }
}