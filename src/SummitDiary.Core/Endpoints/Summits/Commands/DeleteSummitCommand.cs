using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Summits.Commands
{
    public class DeleteSummitCommand : IRequest
    {
        public int SummitId { get; }

        public DeleteSummitCommand(int summitId)
        {
            SummitId = summitId;
        }
    }

    public class DeleteSummitCommandHandler : IRequestHandler<DeleteSummitCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSummitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteSummitCommand request, CancellationToken cancellationToken)
        {
            var summit = await _context.Summits.FirstOrDefaultAsync(x => x.Id == request.SummitId, cancellationToken);
            if (summit == null)
                throw new NotFoundException(nameof(Summit), request.SummitId);

            _context.Summits.Remove(summit);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}