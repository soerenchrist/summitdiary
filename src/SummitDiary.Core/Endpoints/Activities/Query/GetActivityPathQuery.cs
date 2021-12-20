using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Gpx.Dto;
using SummitDiary.Core.Services;

namespace SummitDiary.Core.Endpoints.Activities.Query
{
    public class GetActivityPathQuery : IRequest<AnalysisResultDto>
    {
        public int ActivityId { get; }

        public GetActivityPathQuery(int activityId)
        {
            ActivityId = activityId;
        }
    }

    public class GetActivityPathQueryHandler : IRequestHandler<GetActivityPathQuery, AnalysisResultDto>
    {
        private readonly IApplicationDbContext _context;

        public GetActivityPathQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<AnalysisResultDto> Handle(GetActivityPathQuery request, CancellationToken cancellationToken)
        {
            var activity =
                await _context.Activities
                    .Include(x => x.Attachments)
                    .FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);
            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.ActivityId);

            var gpxAttachment = activity.Attachments!.FirstOrDefault(x => x.FileType == FileType.Gpx);
            if (gpxAttachment == null)
                throw new NotFoundException(nameof(Attachment), request.ActivityId);

            var stream = File.OpenRead(gpxAttachment.FilePath);
            
            var analyzer = new GpxAnalyzer();
            var compressValue = 5;
            return analyzer.AnalyzeGpx(stream, compressValue);
        }
    }
}