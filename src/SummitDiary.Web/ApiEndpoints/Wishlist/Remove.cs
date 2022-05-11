using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Wishlist;

public class Remove : EndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    private readonly IRepository<WishlistItem> _wishlistRepository;

    public Remove(IRepository<WishlistItem> wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }
    
    
    [HttpDelete("/api/wishlist/{wishlistItemId:int}")]
    [SwaggerOperation(
        Summary = "Remove item from wishlist",
        Description = "Remove given item from wishlist",
        OperationId = "Wishlist.Remove",
        Tags = new[]{"WishlistEndpoints"})]
    public override async Task<ActionResult> HandleAsync(int wishlistItemId, CancellationToken cancellationToken = new CancellationToken())
    {
        var item = await _wishlistRepository.GetByIdAsync(wishlistItemId, cancellationToken);
        if (item == null)
            return NotFound();

        await _wishlistRepository.DeleteAsync(item, cancellationToken);

        return Ok();
    }
}