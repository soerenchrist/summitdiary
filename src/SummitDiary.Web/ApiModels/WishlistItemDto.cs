using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.SummitAggregate;

namespace SummitDiary.Web.ApiModels;
public class WishlistItemDto : IMapFrom<WishlistItem>
{
    public int Id { get; set; }
    public SummitDto? Summit { get; set; }
    public bool Finished { get; set; }
}