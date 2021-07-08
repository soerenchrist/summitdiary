using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bogus.DataSets;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Activities.Dto;

namespace SummitDiary.Core.Endpoints.Activities.Query
{
    public class GetActivityByIdQuery : IRequest<ActivityDto>
    {
        public int Id { get; }

        public GetActivityByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ActivityDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetActivityByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ActivityDto> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .Include(x => x.Summits)
                .ThenInclude(x => x.Region)
                .Include(x => x.Summits)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.Id);

            return _mapper.Map<ActivityDto>(activity);
        }
    }
}