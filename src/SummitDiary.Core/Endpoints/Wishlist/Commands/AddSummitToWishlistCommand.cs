using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Wishlist.Dto;

namespace SummitDiary.Core.Endpoints.Wishlist.Commands
{
    public class AddSummitToWishlistCommand : IRequest<WishlistItemDto>
    {
        public int SummitId { get; set; }
    }
    
    public class AddSummitToWishlistCommandHandler : IRequestHandler<AddSummitToWishlistCommand, WishlistItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddSummitToWishlistCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    
        public async Task<WishlistItemDto> Handle(AddSummitToWishlistCommand request, CancellationToken cancellationToken)
        {
            var summit = await _context.Summits.FirstOrDefaultAsync(x => x.Id == request.SummitId, cancellationToken);
            if (summit == null)
                throw new NotFoundException(nameof(Summit), request.SummitId);

            var existing = await _context.WishlistItems.FirstOrDefaultAsync(x => x.SummitId == request.SummitId 
                && !x.Finished, cancellationToken);
            if (existing != null)
                return _mapper.Map<WishlistItemDto>(existing);

            var item = new WishlistItem
            {
                Summit = summit,
                Finished = false,
            };
            await _context.WishlistItems.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<WishlistItemDto>(item);
        }
    }
}