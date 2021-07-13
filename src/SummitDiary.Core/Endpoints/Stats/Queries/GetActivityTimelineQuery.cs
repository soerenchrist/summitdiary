using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Stats.Dto;

namespace SummitDiary.Core.Endpoints.Stats.Queries
{
    public class GetActivityTimelineQuery : IRequest<List<TimelineStatDto>>
    {
        public string ValueType { get; set; } = "elevation";
        public string TimeType { get; set; } = "year";
    }
    
    public class GetActivityTimelineQueryHandler : IRequestHandler<GetActivityTimelineQuery, List<TimelineStatDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetActivityTimelineQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<TimelineStatDto>> Handle(GetActivityTimelineQuery request, CancellationToken cancellationToken)
        {
            if (!_context.Activities.Any())
                return new List<TimelineStatDto>();

            var (currentDate, interval) = request.TimeType switch
            {
                "week" => (DateTime.Now.AddDays(-7), 7),
                "month" => (DateTime.Now.AddMonths(-1), 7),
                "all" => (_context.Activities.OrderByDescending(x => x.HikeDate).First().HikeDate, 365),
                _ => (DateTime.Now.AddYears(-1), 30)
            };

            Expression<Func<Activity, double>> selector = request.ValueType switch
            {
                "distance" => activity => activity.Distance,
                _ => activity => activity.ElevationUp
            };

            var tasks = new List<Task<double>>();
            var results = new List<TimelineStatDto>();
            while (currentDate <= DateTime.Now)
            {
                var nextDate = currentDate.AddDays(interval);

                var date = currentDate;
                var sumsTask = _context.Activities.Where(x => x.HikeDate >= date && x.HikeDate < nextDate)
                    .SumAsync(selector, cancellationToken);
                tasks.Add(sumsTask);
                results.Add(new TimelineStatDto
                {
                    Date = currentDate
                });
                
                currentDate = nextDate;
            }

            var resultValues = await Task.WhenAll(tasks);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Value = (int)resultValues[i];
            }

            return results;
        }
    }
}