using AutoMapper;
using MediatR;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Endpoints.Regions.Dto;

namespace SummitDiary.Core.Endpoints.Regions.Queries
{
    public class GetRegionsQuery : IRequest<List<RegionDto>>
    {
        
    }

    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, List<RegionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRegionsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Task<List<RegionDto>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
            => _context.Regions.ProjectToListAsync<RegionDto>(_mapper.ConfigurationProvider);
        
    }
}