using SummitDiary.Core.Models.Common;
using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using SummitDiary.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Analyze;

public class AnalyzeGpx : BaseAsyncEndpoint
    .WithRequest<AnalyzeGpxRequest>
    .WithResponse<AnalysisResultDto>
{
    private readonly IMapper _mapper;
    private readonly IReadRepository<Summit> _summitRepository;

    public AnalyzeGpx(IMapper mapper,
        IReadRepository<Summit> summitRepository)
    {
        _mapper = mapper;
        _summitRepository = summitRepository;
    }
    
    [HttpPost("/api/gpx/analyze")]
    [SwaggerOperation(
        Summary = "Analyze a gpx file",
        Description = "Analyze the gpx file for some stats and a proposed summit",
        OperationId = "Gpx.Analyze",
        Tags = new[]{"GpxEndpoints"})]
    public override async Task<ActionResult<AnalysisResultDto>> HandleAsync([FromForm] AnalyzeGpxRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        if (request.File == null)
            throw new ArgumentNullException(nameof(request.File));
            
        var analyzer = new GpxAnalyzer();
        var result = analyzer.AnalyzeGpx(request.File.OpenReadStream());
        result.ProposedSummit = await FindSummit(result.Path);

        var dto = _mapper.Map<AnalysisResultDto>(result);
        return dto;
    }
    
    private async Task<Summit?> FindSummit(List<Waypoint> waypoints)
    {
        Waypoint? maxWaypoint = null;
        foreach (var waypoint in waypoints)
        {
            if (maxWaypoint == null)
                maxWaypoint = waypoint;

            if (maxWaypoint.Elevation < waypoint.Elevation)
                maxWaypoint = waypoint;
        }

        if (maxWaypoint == null)
            return null;
            
        const double offset = 0.01;

        var swLat = maxWaypoint.Latitude - offset;
        var swLon = maxWaypoint.Longitude - offset;

        var neLat = maxWaypoint.Latitude + offset;
        var neLon = maxWaypoint.Longitude + offset;

        var spec = new GetSummitsPaginatedSpec(1, 1, bounds: new Bounds(neLat, neLon, swLat, swLon));

        var summits = await _summitRepository.ListAsync(spec);
        var summit = summits.FirstOrDefault();
        
        return summit;
    }
}