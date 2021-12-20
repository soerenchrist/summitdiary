using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Endpoints.Wishlist.Commands;
using SummitDiary.Core.Endpoints.Wishlist.Dto;
using SummitDiary.Core.Endpoints.Wishlist.Queries;

namespace SummitDiary.Web.Api
{
    public class WishlistController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<WishlistItemDto>>> GetWishlist([FromQuery] GetWishlistQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<WishlistItemDto>> FinishWishlistItem(int id)
        {
            return await Mediator.Send(new FinishWishlistItemCommand(id));
        }
        
        [HttpPost]
        public async Task<ActionResult<WishlistItemDto>> AddSummitToWishlist([FromBody] AddSummitToWishlistCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveWishlistItem(int id)
        {
            await Mediator.Send(new RemoveWishlistItemCommand(id));
            return Ok();
        }
    }
}