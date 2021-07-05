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
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Endpoints
{
    public class GetSummitsWithPaginationQuery : IRequest<PaginatedList<SummitDto>>
    {
        public string SearchText { get; set; } = "";
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool OnlyClimbed { get; set; } = false;
        public bool SortDescending { get; set; } = true;
    }

    public class
        GetSummitsWithPaginationQueryHandler : IRequestHandler<GetSummitsWithPaginationQuery, PaginatedList<SummitDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSummitsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public Task<PaginatedList<SummitDto>> Handle(GetSummitsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var summits = _context.Summits
                .Include(x => x.DiaryEntries);

            IQueryable<Summit> ordered = (request.SortBy, request.SortDescending) switch
            {
                ("height", true) => summits.OrderByDescending(x => x.Height),
                ("name", true) => summits.OrderByDescending(x => x.Name),
                ("height", false) => summits.OrderBy(x => x.Height),
                _ => summits.OrderBy(x => x.Name)
            };

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                ordered = ordered.Where(x => EF.Functions.Like(x.Name, $"%{request.SearchText}%"));
            }

            if (request.OnlyClimbed)
            {
                ordered = ordered.Where(x => x.DiaryEntries.Any());
            }

            return ordered.ProjectTo<SummitDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}