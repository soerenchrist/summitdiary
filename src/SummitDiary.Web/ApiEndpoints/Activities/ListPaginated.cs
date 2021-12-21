using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;
using SummitDiary.Core.Models.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class ListPaginated : BaseAsyncEndpoint
    .WithRequest<ListActivitiesPaginatedRequest>
    .WithResponse<PaginatedList<ActivityDto>>
{
    private readonly IReadRepository<Activity> _activityRepository;
    private readonly IMapper _mapper;

    public ListPaginated(IReadRepository<Activity> activityRepository,
        IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/activities")]
    [SwaggerOperation(
        Summary = "Get activities",
        Description = "Get activities with pagination",
        OperationId = "Activity.ListWithPagination",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult<PaginatedList<ActivityDto>>> HandleAsync([FromQuery] ListActivitiesPaginatedRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var spec = new GetActivitiesPaginatedSpec(
            request.PageSize,
            request.PageNumber,
            request.SortBy ?? "hikeDate",
            request.SortDescending,
            request.SearchText,
            request.SummitId);
        
        var countSpec = new GetActivitiesPaginatedSpec(
            request.SortBy ?? "hikeDate",
            request.SortDescending,
            request.SearchText,
            request.SummitId);
        
        var results = await _activityRepository.ListAsync(spec, cancellationToken);
        var count = await _activityRepository.CountAsync(countSpec, cancellationToken);

        var dtos = _mapper.Map<List<ActivityDto>>(results);

        return new PaginatedList<ActivityDto>(dtos, count, request.PageNumber, request.PageSize);
    }
}