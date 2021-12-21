using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class GetById : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<SummitDto>
{
    private readonly IReadRepository<Summit> _summitRepository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Summit> summitRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _mapper = mapper;
    }
    
    [HttpDelete("/api/summits/{id:int}")]
    [SwaggerOperation(
        Summary = "Get a summit by id",
        Description = "Get a summit by a given id",
        OperationId = "Summit.GetById",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult<SummitDto>> HandleAsync(int id, CancellationToken cancellationToken = new CancellationToken())
    {
        var summit = await _summitRepository.GetBySpecAsync(new GetSummitByIdSpec(id), cancellationToken);
        if (summit == null)
            return NotFound();
        return _mapper.Map<SummitDto>(summit);
    }
}