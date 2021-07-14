using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Wishlist.Dto
{
    public class WishlistItemDto : IMapFrom<WishlistItem>
    {
        public int Id { get; set; }
        public SummitDto Summit { get; set; }
        public bool Finished { get; set; }
    }
}