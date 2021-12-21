using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Countries;

public class Create : BaseAsyncEndpoint
    .WithRequest<CreateCountryRequest>
    .WithResponse<CountryDto>
{
    private readonly IRepository<Country> _countryRepository;
    private readonly IMapper _mapper;

    public Create(IRepository<Country> countryRepository,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }
    
    
    [HttpPost("/api/countries")]
    [SwaggerOperation(
        Summary = "Create a new country",
        Description = "Create a new country",
        OperationId = "Countries.Create",
        Tags = new[]{"CountryEndpoints"})]
    public override async Task<ActionResult<CountryDto>> HandleAsync(CreateCountryRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var existing = await _countryRepository.GetBySpecAsync(new GetCountryByNameSpec(request.Name), cancellationToken);
        if (existing != null)
            return BadRequest($"Country named {request.Name} does already exist");

        var country = new Country
        {
            Name = request.Name
        };

        await _countryRepository.AddAsync(country, cancellationToken);
        return _mapper.Map<CountryDto>(country);
    }
}