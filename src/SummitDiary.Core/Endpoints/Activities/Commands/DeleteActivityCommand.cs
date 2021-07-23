using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Activities.Commands
{
    public class DeleteActivityCommand : IRequest
    {
        public int ActivityId { get; }

        public DeleteActivityCommand(int activityId)
        {
            ActivityId = activityId;
        }
    }
    
    public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteActivityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity =
                await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);
            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.ActivityId);

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}