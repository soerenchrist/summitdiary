using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Stats.Queries
{
    public class GetHighestClimbedSummitQuery : IRequest<SummitDto>
    {
        
    }

    public class GetHighestClimbedSummitQueryHandler : IRequestHandler<GetHighestClimbedSummitQuery, SummitDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetHighestClimbedSummitQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SummitDto> Handle(GetHighestClimbedSummitQuery request, CancellationToken cancellationToken)
        {
            var summit = await _context.Summits
                .Include(x => x.Region)
                .Include(x => x.Country)
                .Include(x => x.DiaryEntries)
                .OrderByDescending(x => x.Height)
                .FirstOrDefaultAsync(x => x.DiaryEntries!.Any(), cancellationToken);

            if (summit == null)
                throw new NotFoundException(nameof(Summit), "");

            return _mapper.Map<SummitDto>(summit);
        }
    }
}