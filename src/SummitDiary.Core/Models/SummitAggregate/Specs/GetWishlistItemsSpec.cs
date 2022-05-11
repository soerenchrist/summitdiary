﻿using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetWishlistItemsSpec : Specification<WishlistItem>
{
    public GetWishlistItemsSpec()
    {
        Query
            .Include(x => x.Summit)
            .ThenInclude(x => x!.Country)
            .Include(x => x.Summit)
            .ThenInclude(x => x!.Region);
    }
}