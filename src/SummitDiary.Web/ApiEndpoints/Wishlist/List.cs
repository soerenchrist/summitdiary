using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Wishlist;

public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<List<WishlistItemDto>>
{
    private readonly IReadRepository<WishlistItem> _wishlistRepository;
    private readonly IMapper _mapper;

    public List(IReadRepository<WishlistItem> wishlistRepository,
        IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    }
    
    [HttpGet("/api/wishlist")]
    [SwaggerOperation(
        Summary = "List wishlist items",
        Description = "List wishlist items",
        OperationId = "Wishlist.List",
        Tags = new[]{"WishlistEndpoints"})]
    public override async Task<ActionResult<List<WishlistItemDto>>> HandleAsync(CancellationToken cancellationToken = new())
    {
        var items = await _wishlistRepository.ListAsync(new GetWishlistItemsSpec(), cancellationToken);
        return _mapper.Map<List<WishlistItemDto>>(items);
    }
}