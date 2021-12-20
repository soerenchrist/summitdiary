using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Regions.Dto;

namespace SummitDiary.Core.Endpoints.Regions.Commands
{
    public class CreateRegionCommand : IRequest<RegionDto>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, RegionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRegionCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<RegionDto> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Regions.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"Region named {request.Name} does already exist");

            var region = new Region
            {
                Name = request.Name
            };

            await _context.Regions.AddAsync(region, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RegionDto>(region);
        }
    }
}