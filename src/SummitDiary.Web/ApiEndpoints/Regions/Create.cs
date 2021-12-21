using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Regions;

public class Create : BaseAsyncEndpoint
    .WithRequest<CreateRegionRequest>
    .WithResponse<RegionDto>
{
    private readonly IRepository<Region> _regionRepository;
    private readonly IMapper _mapper;

    public Create(IRepository<Region> regionRepository,
        IMapper mapper)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    
    [HttpPost("/api/regions")]
    [SwaggerOperation(
        Summary = "Create a new region",
        Description = "Create a new region",
        OperationId = "Regions.Create",
        Tags = new[]{"RegionEndpoints"})]
    public override async Task<ActionResult<RegionDto>> HandleAsync(CreateRegionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var existing = await _regionRepository.GetBySpecAsync(new GetRegionByNameSpec(request.Name), cancellationToken);
        if (existing != null)
            return BadRequest("A region with this name does already exist");

        var region = new Region
        {
            Name = request.Name
        };

        await _regionRepository.AddAsync(region, cancellationToken);

        return _mapper.Map<RegionDto>(region);
    }
}