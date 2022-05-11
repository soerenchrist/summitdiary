using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Wishlist;

public class Create : EndpointBaseAsync
    .WithRequest<CreateWishlistItemRequest>
    .WithActionResult<WishlistItemDto>
{
    private readonly IReadRepository<Summit> _summitRepository;
    private readonly IRepository<WishlistItem> _wishlistRepository;
    private readonly IMapper _mapper;

    public Create(IReadRepository<Summit> summitRepository,
        IRepository<WishlistItem> wishlistRepository,
        IMapper mapper)
    {
        _summitRepository = summitRepository;
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    }
    
    [HttpPost("/api/wishlist")]
    [SwaggerOperation(
        Summary = "Create a wishlist item",
        Description = "Create a wishlist item for the given summit",
        OperationId = "Wishlist.Create",
        Tags = new[]{"Wishlist.Create"})]
    public override async Task<ActionResult<WishlistItemDto>> HandleAsync(CreateWishlistItemRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var summit = await _summitRepository.GetByIdAsync(request.SummitId, cancellationToken);
        if (summit == null)
            return NotFound();

        var wishlistItem = new WishlistItem
        {
            Finished = false,
            SummitId = request.SummitId
        };
        await _wishlistRepository.AddAsync(wishlistItem, cancellationToken);

        return _mapper.Map<WishlistItemDto>(wishlistItem);
    }
}