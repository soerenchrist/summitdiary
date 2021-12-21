using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Wishlist;

public class Finish : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<WishlistItemDto>
{
    private readonly IRepository<WishlistItem> _wishlistRepository;
    private readonly IMapper _mapper;

    public Finish(IRepository<WishlistItem> wishlistRepository,
        IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    }
    
    [HttpPut("/api/wishlist/{wishlistItemId:int}")]
    [SwaggerOperation(
        Summary = "Finish a wishlist item",
        Description = "Finish the given wishlist item",
        OperationId = "Wishlist.Finish",
        Tags = new[]{"Wishlist.Finish"})]
    public override async Task<ActionResult<WishlistItemDto>> HandleAsync(int wishlistItemId, CancellationToken cancellationToken = new CancellationToken())
    {
        var item = await _wishlistRepository.GetByIdAsync(wishlistItemId, cancellationToken);
        if (item == null)
            return NotFound();

        item.Finished = true;
        await _wishlistRepository.UpdateAsync(item, cancellationToken);

        return _mapper.Map<WishlistItemDto>(item);
    }
}