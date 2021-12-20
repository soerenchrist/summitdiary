using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Wishlist.Dto;

namespace SummitDiary.Core.Endpoints.Wishlist.Commands
{
    public class FinishWishlistItemCommand : IRequest<WishlistItemDto>
    {
        public int WishlistItemId { get; }

        public FinishWishlistItemCommand(int wishlistItemId)
        {
            WishlistItemId = wishlistItemId;
        }
    }
    
    public class FinishWishlistItemCommandHandler : IRequestHandler<FinishWishlistItemCommand, WishlistItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FinishWishlistItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<WishlistItemDto> Handle(FinishWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.WishlistItems.FirstOrDefaultAsync(x => x.Id == request.WishlistItemId,
                cancellationToken);
            if (item == null)
                throw new NotFoundException(nameof(WishlistItem), request.WishlistItemId);

            item.Finished = true;
            _context.WishlistItems.Update(item);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<WishlistItemDto>(item);
        }
    }
}