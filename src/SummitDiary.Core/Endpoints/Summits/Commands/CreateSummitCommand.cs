using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Commands
{
    public class CreateSummitCommand : IRequest<SummitDto>
    {
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
    }


    public class CreateSummitCommandHandler : IRequestHandler<CreateSummitCommand, SummitDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateSummitCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        
        public async Task<SummitDto> Handle(CreateSummitCommand request, CancellationToken cancellationToken)
        {
            var country =
                await _context.Countries.FirstOrDefaultAsync(x => x.Id == request.CountryId, cancellationToken);
            if (country == null)
                throw new NotFoundException(nameof(Country), request.CountryId);
            var region =
                await _context.Regions.FirstOrDefaultAsync(x => x.Id == request.RegionId, cancellationToken);
            if (region == null)
                throw new NotFoundException(nameof(Region), request.RegionId);
            
            var summit = new Summit
            {
                Height = request.Height,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Name = request.Name,
                CountryId = request.CountryId,
                RegionId = request.RegionId
            };

            await _context.Summits.AddAsync(summit, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<SummitDto>(summit);
        }
    }
}