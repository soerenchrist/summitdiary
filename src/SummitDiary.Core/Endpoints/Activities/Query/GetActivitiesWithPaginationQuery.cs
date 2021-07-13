using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Common.Models.Common;
using SummitDiary.Core.Endpoints.Activities.Dto;

namespace SummitDiary.Core.Endpoints.Activities.Query
{
    public class GetActivitiesWithPaginationQuery : IRequest<PaginatedList<ActivityDto>>
    {
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? SummitId { get; set; }
        public bool SortDescending { get; set; }
    }

    public class
        GetActivitiesWithPaginationQueryHandler : IRequestHandler<GetActivitiesWithPaginationQuery,
            PaginatedList<ActivityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetActivitiesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Task<PaginatedList<ActivityDto>> Handle(GetActivitiesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var diaryEntries = _context.Activities
                .Include(x => x.Summits);

            IQueryable<Activity> ordered = (request.SortBy, request.SortDescending) switch
            {
                ("title", true) => diaryEntries.OrderByDescending(x => x.Title),
                ("title", false) => diaryEntries.OrderBy(x => x.Title),
                ("hikeDate", true) => diaryEntries.OrderByDescending(x => x.HikeDate),
                ("hikeDate", false) => diaryEntries.OrderBy(x => x.HikeDate),
                ("elevationUp", true) => diaryEntries.OrderByDescending(x => x.ElevationUp),
                ("elevationUp", false) => diaryEntries.OrderBy(x => x.ElevationUp),
                ("elevationDown", true) => diaryEntries.OrderByDescending(x => x.ElevationDown),
                ("elevationDown", false) => diaryEntries.OrderBy(x => x.ElevationDown),
                ("distance", true) => diaryEntries.OrderByDescending(x => x.Distance),
                ("distance", false) => diaryEntries.OrderBy(x => x.Distance),
                ("duration", true) => diaryEntries.OrderByDescending(x => x.Duration),
                _ => diaryEntries.OrderBy(x => x.Duration),
            };

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                ordered = ordered.Where(x => EF.Functions.Like(x.Title, $"%{request.SearchText}%"));
            }

            if (request.SummitId != null)
            {
                ordered = ordered.Where(x => x.Summits.Any(x => x.Id == request.SummitId));
            }

            return ordered.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}