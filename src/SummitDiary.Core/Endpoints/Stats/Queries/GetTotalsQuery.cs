using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Endpoints.Stats.Dto;

namespace SummitDiary.Core.Endpoints.Stats.Queries
{
    public class GetTotalsQuery : IRequest<TotalsDto>
    {
        
    }
    
    public class GetTotalsQueryHandler : IRequestHandler<GetTotalsQuery, TotalsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetTotalsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<TotalsDto> Handle(GetTotalsQuery request, CancellationToken cancellationToken)
        {
            var distance = await _context.Activities.SumAsync(x => x.Distance, cancellationToken);
            var elevation = await _context.Activities.SumAsync(x => x.ElevationUp, cancellationToken);
            var duration = await _context.Activities.SumAsync(x => x.Duration, cancellationToken);
            var activityCount = await _context.Activities.CountAsync(cancellationToken);
            var summitCount = await _context.Activities.SelectMany(x => x.Summits!).Distinct()
                .CountAsync(cancellationToken);
            
            return new TotalsDto
            {
                Distance = (int)distance,
                Elevation = (int)elevation,
                Duration = duration,
                ActivityCount = activityCount,
                SummitCount = summitCount
            };
        }
    }
}