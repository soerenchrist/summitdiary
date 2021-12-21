using SummitDiary.Core.Models.Common;
using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class ListPaginated : BaseAsyncEndpoint
    .WithRequest<ListSummitsPaginatedRequest>
    .WithResponse<PaginatedList<SummitDto>>
{
    private readonly IRepository<Summit> _summitRepository;
    private readonly IMapper _mapper;

    public ListPaginated(IRepository<Summit> summitRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/summits")]
    [SwaggerOperation(
        Summary = "List summits",
        Description = "List summits with pagination",
        OperationId = "Summit.ListPaginated",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult<PaginatedList<SummitDto>>> HandleAsync([FromQuery] ListSummitsPaginatedRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        Bounds? bounds = null;
        if (request.NeLat != null 
            && request.NeLon != null 
            && request.SwLat != null 
            && request.SwLon != null)
        {
            bounds = new Bounds(
                request.NeLat.Value, 
                request.NeLon.Value,
                request.SwLat.Value, 
                request.SwLon.Value);
        }

        var spec = new GetSummitsPaginatedSpec(
            request.PageSize,
            request.PageNumber,
            request.SortBy ?? "name",
            request.SortDescending,
            request.SearchText,
            request.OnlyClimbed,
            bounds);
        
        var count = await _summitRepository.CountAsync(cancellationToken);
        var summits = await _summitRepository.ListAsync(spec, cancellationToken);
        var dtos = _mapper.Map<List<SummitDto>>(summits);
        return new PaginatedList<SummitDto>(dtos, count, request.PageNumber, request.PageSize);
    }
}