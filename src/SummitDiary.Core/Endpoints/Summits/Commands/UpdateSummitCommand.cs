using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Commands
{
    public class UpdateSummitCommand : IRequest<SummitDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
    }
    
    public class UpdateSummitCommandHandler : IRequestHandler<UpdateSummitCommand, SummitDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateSummitCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SummitDto> Handle(UpdateSummitCommand request, CancellationToken cancellationToken)
        {
            var country =
                await _context.Countries.FirstOrDefaultAsync(x => x.Id == request.CountryId, cancellationToken);
            if (country == null)
                throw new NotFoundException(nameof(Country), request.CountryId);
            var region =
                await _context.Regions.FirstOrDefaultAsync(x => x.Id == request.RegionId, cancellationToken);
            if (region == null)
                throw new NotFoundException(nameof(Region), request.RegionId);

            var existing = await _context.Summits.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (existing == null)
                throw new NotFoundException(nameof(Summit), request.Id);

            existing.Height = request.Height;
            existing.Latitude = request.Latitude;
            existing.Longitude = request.Longitude;
            existing.Name = request.Name;
            existing.CountryId = request.CountryId;
            existing.RegionId = request.RegionId;

            _context.Summits.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<SummitDto>(existing);
        }
    }
}