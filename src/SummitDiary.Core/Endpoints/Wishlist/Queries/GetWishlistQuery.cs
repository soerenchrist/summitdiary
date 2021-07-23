using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Endpoints.Wishlist.Dto;

namespace SummitDiary.Core.Endpoints.Wishlist.Queries
{
    public class GetWishlistQuery : IRequest<List<WishlistItemDto>>
    {
        public bool Finished { get; set; } = false;
    }
    
    public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, List<WishlistItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWishlistQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Task<List<WishlistItemDto>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            return _context.WishlistItems.Where(x => !x.Finished)
                .ProjectToListAsync<WishlistItemDto>(_mapper.ConfigurationProvider);
        }
    }
}