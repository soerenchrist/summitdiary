using SummitDiary.Core.Models.Common;
using SummitDiary.Core.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Analyze;

public class AnalyzePath : BaseAsyncEndpoint
    .WithRequest<AnalyzePathRequest>
    .WithResponse<AnalysisResultDto>
{
    private readonly IElevationService _elevationService;
    private readonly IGpxAnalyzer _analyzer;
    private readonly IMapper _mapper;

    public AnalyzePath(IElevationService elevationService,
        IGpxAnalyzer analyzer,
        IMapper mapper)
    {
        _elevationService = elevationService;
        _analyzer = analyzer;
        _mapper = mapper;
    }
    
    [HttpPost("/api/gpx/analyzepath")]
    [SwaggerOperation(
        Summary = "Analyze a path",
        Description = "Analyze the path for some stats and a proposed summit",
        OperationId = "Gpx.AnalyzePath",
        Tags = new[]{"GpxEndpoints"})]
    public override async Task<ActionResult<AnalysisResultDto>> HandleAsync(AnalyzePathRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var waypoints = new List<Waypoint>();
        foreach (var point in request.Points)
        {
            var elevation = await _elevationService.GetElevation(point.Latitude, point.Longitude, cancellationToken);
            if (elevation == null)
                continue;

            var waypoint = new Waypoint(point.Latitude, point.Longitude, elevation.Value, DateTime.Now);
            waypoints.Add(waypoint);
        }

        var result = _analyzer.AnalyzePath(waypoints);
        return _mapper.Map<AnalysisResultDto>(result);
    }
}