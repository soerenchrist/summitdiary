using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Endpoints.Stats.Dto;

namespace SummitDiary.Core.Endpoints.Stats.Queries
{
    public class GetSummitHeightStatsQuery : IRequest<List<BaseStatDto>>
    {
        
    }
    
    public class GetSummitHeightStatsQueryHandler : IRequestHandler<GetSummitHeightStatsQuery, List<BaseStatDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetSummitHeightStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<BaseStatDto>> Handle(GetSummitHeightStatsQuery request, CancellationToken cancellationToken)
        {
            const int start = 2000;
            const int steps = 250;
            var climbedSummits = _context
                .Summits
                .Include(x => x.DiaryEntries)
                .Where(x => x.DiaryEntries!.Any());

            if (!climbedSummits.Any())
                return new List<BaseStatDto>();
            
            var results = new List<BaseStatDto>();
            var highest = await climbedSummits.MaxAsync(x => x.Height, cancellationToken);
            for (int currentLower = start; currentLower < highest; currentLower += steps)
            {
                var summitCount =
                    await climbedSummits.CountAsync(x => x.Height >= currentLower && x.Height < currentLower + steps, cancellationToken);
                results.Add(new BaseStatDto
                {
                    Name = $"{currentLower} - {currentLower+steps} m",
                    Value = summitCount
                });
            }

            return results;
        }
    }
}