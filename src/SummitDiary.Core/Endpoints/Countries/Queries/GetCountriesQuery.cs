using AutoMapper;
using MediatR;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Endpoints.Countries.Dto;

namespace SummitDiary.Core.Endpoints.Countries.Queries
{
    public class GetCountriesQuery : IRequest<List<CountryDto>>
    {
        
    }

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
            => _context.Countries.ProjectToListAsync<CountryDto>(_mapper.ConfigurationProvider);
        
    }
}