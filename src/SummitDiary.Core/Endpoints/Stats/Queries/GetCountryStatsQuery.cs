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
    public class GetCountryStatsQuery : IRequest<List<BaseStatDto>>
    {
        public string ValueType { get; set; } = "elevation";
    }
    
    public class GetCountryStatsQueryHandler : IRequestHandler<GetCountryStatsQuery, List<BaseStatDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCountryStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<BaseStatDto>> Handle(GetCountryStatsQuery request, CancellationToken cancellationToken)
        {
            var activities = _context.Activities.Include(x => x.Summits)
                .ThenInclude(x => x.Country);

            Expression<Func<Activity, double>> selector = request.ValueType switch
            {
                "distance" => activity => activity.Distance,
                _ => activity => activity.ElevationUp
            };

            var results = new List<BaseStatDto>();
            var countries = activities.SelectMany(x => x.Summits).Select(x => x.Country).Distinct();
            foreach (var country in countries)
            {
                var value = await activities.Where(x => x.Summits.Any(s => s.CountryId == country.Id))
                    .SumAsync(selector, cancellationToken);
                results.Add(new BaseStatDto
                {
                    Name = country.Name,
                    Value = value
                });
            }

            return results;
        }
    }
}