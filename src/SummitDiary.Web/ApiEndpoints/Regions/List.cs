using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Regions;

public class List : BaseAsyncEndpoint
    .WithoutRequest
    .WithResponse<List<RegionDto>>
{
    private readonly IReadRepository<Region> _regionRepository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Region> regionRepository,
        IMapper mapper)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    
    [HttpGet("/api/regions")]
    [SwaggerOperation(
        Summary = "List all regions",
        Description = "List all regions",
        OperationId = "Regions.List",
        Tags = new[]{"RegionEndpoints"})]
    public override async Task<ActionResult<List<RegionDto>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var regions = await _regionRepository.ListAsync(cancellationToken);
        return _mapper.Map<List<RegionDto>>(regions);
    }
}