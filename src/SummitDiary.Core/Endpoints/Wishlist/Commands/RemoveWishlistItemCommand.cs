using System.Threading;
using System.Threading.Tasks;
using Bogus.DataSets;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Wishlist.Commands
{
    public class RemoveWishlistItemCommand : IRequest
    {
        public int WishlistItemId { get; }

        public RemoveWishlistItemCommand(int wishlistItemId)
        {
            WishlistItemId = wishlistItemId;
        }
    }
    
    public class RemoveWishlistItemCommandHandler : IRequestHandler<RemoveWishlistItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public RemoveWishlistItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(RemoveWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.WishlistItems.FirstOrDefaultAsync(x => x.Id == request.WishlistItemId,
                cancellationToken);
            if (item == null)
                throw new NotFoundException(nameof(WishlistItem), request.WishlistItemId);

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}