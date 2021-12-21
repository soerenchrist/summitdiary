using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class Create : BaseAsyncEndpoint
    .WithRequest<CreateSummitRequest>
    .WithResponse<SummitDto>
{
    private readonly IRepository<Summit> _summitRepository;
    private readonly IReadRepository<Country> _countryRepository;
    private readonly IReadRepository<Region> _regionRepository;
    private readonly IMapper _mapper;

    public Create(IRepository<Summit> summitRepository,
        IReadRepository<Country> countryRepository,
        IReadRepository<Region> regionRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _countryRepository = countryRepository;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    [HttpPost("/api/summits")]
    [SwaggerOperation(
        Summary = "Creates a new summit",
        Description = "Creates a new summit",
        OperationId = "Summit.Create",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult<SummitDto>> HandleAsync(CreateSummitRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var country = await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken);
        if (country == null)
            return NotFound();

        var region = await _regionRepository.GetByIdAsync(request.RegionId, cancellationToken);
        if (region == null)
            return NotFound();
        
        var summit = new Summit
        {
            Height = request.Height,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Name = request.Name,
            CountryId = request.CountryId,
            RegionId = request.RegionId
        };

        await _summitRepository.AddAsync(summit, cancellationToken);

        return _mapper.Map<SummitDto>(summit);
    }
}