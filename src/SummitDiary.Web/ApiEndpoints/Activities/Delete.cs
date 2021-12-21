using SummitDiary.Core.Models.ActivityAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class Delete : BaseAsyncEndpoint
    .WithRequest<int>
    .WithoutResponse
{
    private readonly IRepository<Activity> _activityRepository;

    public Delete(IRepository<Activity> activityRepository)
    {
        _activityRepository = activityRepository;
    }
    
    [HttpDelete("/api/activities/{activityId:int}")]
    [SwaggerOperation(
        Summary = "Delete activity by id",
        Description = "Delete activity by id",
        OperationId = "Activity.Delete",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult> HandleAsync(int activityId, CancellationToken cancellationToken = new CancellationToken())
    {
        var activity = await _activityRepository.GetByIdAsync(activityId, cancellationToken);
        if (activity == null)
            return NotFound();

        await _activityRepository.DeleteAsync(activity, cancellationToken);
        return Ok();
    }
}