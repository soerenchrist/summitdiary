using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Countries;

public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<CountryDto>>
{
    private readonly IReadRepository<Country> _countryRepository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Country> countryRepository,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/countries")]
    [SwaggerOperation(
        Summary = "List all countries",
        Description = "List all countries",
        OperationId = "Countries.List",
        Tags = new[]{"CountryEndpoints"})]
    public override async Task<ActionResult<List<CountryDto>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var countries = await _countryRepository.ListAsync(cancellationToken);
        return _mapper.Map<List<CountryDto>>(countries);
    }
}