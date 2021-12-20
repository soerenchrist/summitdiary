using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Queries
{
    public class GetSummitByIdQuery : IRequest<SummitDto>
    {
        public int SummitId { get; }
        public GetSummitByIdQuery(int summitId)
        {
            SummitId = summitId;
        }
    }

    public class GetSummitByIdQueryHandler : IRequestHandler<GetSummitByIdQuery, SummitDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSummitByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SummitDto> Handle(GetSummitByIdQuery request, CancellationToken cancellationToken)
        {
            var summit = await _context.Summits.ProjectTo<SummitDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.SummitId, cancellationToken);
            if (summit == null)
                throw new NotFoundException(nameof(Summit), request.SummitId);

            return summit;
        }
    }
    
}