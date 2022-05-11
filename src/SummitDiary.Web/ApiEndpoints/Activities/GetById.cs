using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class GetById : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<ActivityDto>
{
    private readonly IReadRepository<Activity> _activityRepository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Activity> activityRepository,
        IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/activities/{activityId:int}")]
    [SwaggerOperation(
        Summary = "Get activity by id",
        Description = "Get activity by id",
        OperationId = "Activity.GetById",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult<ActivityDto>> HandleAsync(int activityId, CancellationToken cancellationToken = new CancellationToken())
    {
        var activity = await _activityRepository.GetBySpecAsync(new GetActivityByIdSpec(activityId), cancellationToken);
        if (activity == null)
            return NotFound();

        return _mapper.Map<ActivityDto>(activity);
    }
}