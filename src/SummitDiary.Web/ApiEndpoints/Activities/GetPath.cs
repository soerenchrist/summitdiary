using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;
using SummitDiary.Core.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class GetPath : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<AnalysisResultDto>
{
    private readonly IGpxAnalyzer _gpxAnalyzer;
    private readonly IReadRepository<Activity> _activityRepository;
    private readonly IMapper _mapper;

    public GetPath(IGpxAnalyzer gpxAnalyzer,
        IReadRepository<Activity> activityRepository,
        IMapper mapper)
    {
        _gpxAnalyzer = gpxAnalyzer;
        _activityRepository = activityRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/activities/{activityId:int}/path")]
    [SwaggerOperation(
        Summary = "Get path for activity",
        Description = "Get path for given activity",
        OperationId = "Activity.GetPath",
        Tags = new[]{"ActivityEndpoints"})]
    public override async Task<ActionResult<AnalysisResultDto>> HandleAsync(int activityId, CancellationToken cancellationToken = new CancellationToken())
    {
        var activity = await _activityRepository.GetBySpecAsync(new GetActivityByIdSpec(activityId), cancellationToken);
        if (activity == null)
            return NotFound();
        var gpxAttachment = activity.Attachments?.FirstOrDefault(x => x.FileType == FileType.Gpx);
        if (gpxAttachment == null)
            return NotFound();
        var stream = System.IO.File.OpenRead(gpxAttachment.FilePath);
            
        const int compressValue = 5;
        var result = _gpxAnalyzer.AnalyzeGpx(stream, compressValue);
        return _mapper.Map<AnalysisResultDto>(result);
    }
}