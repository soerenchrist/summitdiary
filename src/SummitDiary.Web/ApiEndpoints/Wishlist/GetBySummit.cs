using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Wishlist;

public class GetBySummit : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<WishlistItemDto>
{
    private readonly IReadRepository<WishlistItem> _wishlistRepository;
    private readonly IMapper _mapper;

    public GetBySummit(IReadRepository<WishlistItem> wishlistRepository,
        IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    } 
    
    [HttpGet("/api/summits/{summitId:int}/wishlist")]
    [SwaggerOperation(
        Summary = "Get wishlist item for given summit",
        Description = "Get wishlist item for given summit",
        OperationId = "Wishlist.GetBySummit",
        Tags = new[]{"WishlistEndpoints"})]
    public override async Task<ActionResult<WishlistItemDto>> HandleAsync([FromRoute] int summitId, CancellationToken cancellationToken = new CancellationToken())
    {
        var item = await _wishlistRepository.GetBySpecAsync(new GetWishlistItemBySummitSpec(summitId), cancellationToken);
        if (item == null)
            return NotFound();

        return _mapper.Map<WishlistItemDto>(item);
    }
}