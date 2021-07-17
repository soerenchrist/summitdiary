using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bogus.DataSets;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Wishlist.Dto;

namespace SummitDiary.Core.Endpoints.Wishlist.Queries
{
    public class GetWishlistItemForSummitQuery : IRequest<WishlistItemDto>
    {
        public int SummitId { get; }

        public GetWishlistItemForSummitQuery(int summitId)
        {
            SummitId = summitId;
        }
    }
    
    public class GetWishlistItemForSummitQueryHandler : IRequestHandler<GetWishlistItemForSummitQuery, WishlistItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWishlistItemForSummitQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<WishlistItemDto> Handle(GetWishlistItemForSummitQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.WishlistItems
                .Include(x => x.Summit)
                .FirstOrDefaultAsync(x => x.SummitId == request.SummitId,
                cancellationToken);
            if (item == null)
                throw new NotFoundException(nameof(WishlistItem), request.SummitId);

            return _mapper.Map<WishlistItemDto>(item);
        }
    }
}