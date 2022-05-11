using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetHighestSummit : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<SummitDto>
{
    private readonly IReadRepository<Summit> _summitRepository;
    private readonly IMapper _mapper;

    public GetHighestSummit(IReadRepository<Summit> summitRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _mapper = mapper;
    }
    
    [HttpGet("highestClimbed")]
    public override async Task<ActionResult<SummitDto>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var summit = await _summitRepository.GetBySpecAsync(new GetHighestSummitSpec(), cancellationToken);
        if (summit == null)
            return NotFound();

        return _mapper.Map<SummitDto>(summit);
    }
}