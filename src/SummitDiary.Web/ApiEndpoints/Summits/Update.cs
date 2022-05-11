using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateSummitRequest>
    .WithActionResult<SummitDto>
{
    private readonly IRepository<Summit> _summitRepository;
    private readonly IReadRepository<Country> _countryRepository;
    private readonly IReadRepository<Region> _regionRepository;
    private readonly IMapper _mapper;

    public Update(IRepository<Summit> summitRepository,
        IReadRepository<Country> countryRepository,
        IReadRepository<Region> regionRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _countryRepository = countryRepository;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    [HttpPut("/api/summits")]
    [SwaggerOperation(
        Summary = "Updates a summit",
        Description = "Deletes the summit with the given data",
        OperationId = "Summit.Update",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult<SummitDto>> HandleAsync([FromBody] UpdateSummitRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var country =
            await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken);
        if (country == null)
            return NotFound();
        var region =
            await _regionRepository.GetByIdAsync(request.RegionId, cancellationToken);
        if (region == null)
            return NotFound();

        var existing = await _summitRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return NotFound();

        existing.Height = request.Height;
        existing.Latitude = request.Latitude;
        existing.Longitude = request.Longitude;
        existing.Name = request.Name;
        existing.CountryId = request.CountryId;
        existing.RegionId = request.RegionId;

        await _summitRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<SummitDto>(existing);
    }
}