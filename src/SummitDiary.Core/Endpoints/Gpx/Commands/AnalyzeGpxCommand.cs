using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Gpx.Dto;
using SummitDiary.Core.Services;

namespace SummitDiary.Core.Endpoints.Gpx.Commands
{
    public class AnalyzeGpxCommand : IRequest<AnalysisResultDto>
    {
        public IFormFile File { get; set; }
    }

    public class AnalyzeGpxCommandHandler : IRequestHandler<AnalyzeGpxCommand, AnalysisResultDto>
    {
        private readonly IApplicationDbContext _context;

        public AnalyzeGpxCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<AnalysisResultDto> Handle(AnalyzeGpxCommand request, CancellationToken cancellationToken)
        {
            var analyzer = new GpxAnalyzer();
            var dto = analyzer.AnalyzeGpx(request.File.OpenReadStream());
            dto.ProposedSummit = await FindSummit(dto.Path);
            return dto;
        }

        private Task<Summit> FindSummit(List<Waypoint> waypoints)
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

            return _context.Summits
                .Include(x => x.Region)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Latitude > lowerLat && x.Latitude < upperLat
                                                                       && x.Longitude > lowerLon &&
                                                                       x.Longitude < upperLon);
        }
    }
    
    static class DistanceAlgorithm
    {
        const double Radius = 6371;

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        public static double Radians(double x)
        {
            return x * Math.PI / 180;
        }

        /// <summary>
        /// Calculate the distance between two places.
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double DistanceBetweenPlaces(
            double lon1,
            double lat1,
            double lon2,
            double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * Radius;
        }

    }
    
}