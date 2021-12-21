using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetWishlistItemBySummitSpec : Specification<WishlistItem>, ISingleResultSpecification
{
    public GetWishlistItemBySummitSpec(int summitId)
    {
        Query
            .Include(x => x.Summit)
            .Where(x => x.SummitId == summitId);
    }
}