using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Endpoints.Gpx.Dto;
using SummitDiary.Core.Services;

namespace SummitDiary.Core.Endpoints.Gpx.Commands
{
    public class AnalyzePathCommand : IRequest<AnalysisResultDto>
    {
        public List<PathPointDto> Points { get; set; }
    }

    public class AnalyzePathCommandHandler : IRequestHandler<AnalyzePathCommand, AnalysisResultDto>
    {
        private readonly IElevationService _elevationService;

        public AnalyzePathCommandHandler(IElevationService elevationService)
        {
            _elevationService = elevationService;
        }

        public async Task<AnalysisResultDto> Handle(AnalyzePathCommand request, CancellationToken cancellationToken)
        {
            var waypoints = new List<Waypoint>();
            foreach (var point in request.Points)
            {
                var elevation = await _elevationService.GetElevation(point.Latitude, point.Longitude, cancellationToken);
                if (elevation == null)
                    continue;

                var waypoint = new Waypoint(point.Latitude, point.Longitude, elevation.Value, DateTime.Now);
                waypoints.Add(waypoint);
            }

            var analyzer = new GpxAnalyzer();
            return analyzer.AnalyzePath(waypoints);
        }
    }
    
}