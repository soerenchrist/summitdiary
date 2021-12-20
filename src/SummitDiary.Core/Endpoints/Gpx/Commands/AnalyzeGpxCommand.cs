using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Endpoints.Gpx.Dto;
using SummitDiary.Core.Endpoints.Summits.Dto;
using SummitDiary.Core.Services;

namespace SummitDiary.Core.Endpoints.Gpx.Commands
{
    public class AnalyzeGpxCommand : IRequest<AnalysisResultDto>
    {
        public IFormFile? File { get; set; }
    }

    public class AnalyzeGpxCommandHandler : IRequestHandler<AnalyzeGpxCommand, AnalysisResultDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AnalyzeGpxCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<AnalysisResultDto> Handle(AnalyzeGpxCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null)
                throw new ArgumentNullException(nameof(request.File));
            
            var analyzer = new GpxAnalyzer();
            var dto = analyzer.AnalyzeGpx(request.File.OpenReadStream());
            dto.ProposedSummit = await FindSummit(dto.Path);
            return dto;
        }

        private async Task<SummitDto?> FindSummit(List<Waypoint> waypoints)
        {
            Waypoint? maxWaypoint = null;
            foreach (var waypoint in waypoints)
            {
                if (maxWaypoint == null)
                    maxWaypoint = waypoint;

                if (maxWaypoint.Value.Elevation < waypoint.Elevation)
                    maxWaypoint = waypoint;
            }

            if (maxWaypoint == null)
                return null;
            
            const double offset = 0.01;

            var lowerLat = maxWaypoint.Value.Latitude - offset;
            var lowerLon = maxWaypoint.Value.Longitude - offset;

            var upperLat = maxWaypoint.Value.Latitude + offset;
            var upperLon = maxWaypoint.Value.Longitude + offset;

            var summit = await _context.Summits
                .Include(x => x.Region)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Latitude > lowerLat && x.Latitude < upperLat
                                                                       && x.Longitude > lowerLon &&
                                                                       x.Longitude < upperLon);
            if (summit != null)
            {
                return _mapper.Map<SummitDto>(summit);
            }

            return null;
        }
    }
}