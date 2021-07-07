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
            using var xmlReader = new XmlTextReader(request.File.OpenReadStream());
            var file = GpxFile.ReadFrom(xmlReader, new GpxReaderSettings
            {
                TimeZoneInfo = TimeZoneInfo.Local
            });

            double totalElevationUp = 0.0;
            double totalElevationDown = 0.0;
            double totalDistance = 0.0;

            var firstTrack = file.Tracks.FirstOrDefault();
            if (firstTrack == null)
                throw new InvalidDataException("Invalid gpx file");

            var waypoints = file.Tracks.SelectMany(x => x.Segments)
                .SelectMany(x => x.Waypoints)
                .ToList();

            if (waypoints.Count == 0)
                throw new InvalidDataException("Invalid gpx file");

            var path = new List<Coordinate>();
            
            for (var i = 0; i < waypoints.Count - 1; i++)
            {
                var current = waypoints[i];
                var next = waypoints[i + 1];

                if (current.ElevationInMeters.HasValue && next.ElevationInMeters.HasValue)
                {
                    var elevationDiff = next.ElevationInMeters.Value - current.ElevationInMeters.Value;
                    if (elevationDiff > 0)
                        totalElevationUp += elevationDiff;
                    else
                        totalElevationDown += elevationDiff;
                }

                var distance = DistanceAlgorithm.DistanceBetweenPlaces(current.Longitude, current.Latitude, next.Longitude,
                    next.Latitude);

                totalDistance += distance;
                path.Add(new Coordinate(current.Latitude, current.Longitude));
            }

            var startPoint = waypoints.First();
            var endPoint = waypoints.Last();
            
            var startTime = ParseTimeStamp(startPoint.TimestampUtc);
            var endTime = ParseTimeStamp(endPoint.TimestampUtc);

            return new AnalysisResultDto
            {
                Distance = totalDistance,
                ElevationDown = (int) totalElevationDown * -1,
                ElevationUp = (int) totalElevationUp,
                HikeDate = waypoints.First().TimestampUtc?.Date,
                StartTime = startTime,
                EndTime = endTime,
                ProposedTitle = firstTrack.Name,
                ProposedSummit = await FindSummit(waypoints),
                StartPoint = new Coordinate(startPoint.Latitude, startPoint.Longitude),
                EndPoint = new Coordinate(startPoint.Latitude, startPoint.Longitude),
                Path = path
            };
        }

        private string ParseTimeStamp(DateTime? timestampUtc)
        {
            if (timestampUtc == null)
                return null;
            var date = timestampUtc.Value.ToLocalTime();
            return date.ToShortTimeString();
        }

        private Task<Summit> FindSummit(List<GpxWaypoint> waypoints)
        {
            GpxWaypoint maxWaypoint = null;
            foreach (var waypoint in waypoints)
            {
                if (maxWaypoint == null)
                    maxWaypoint = waypoint;

                if (maxWaypoint.ElevationInMeters < waypoint.ElevationInMeters)
                    maxWaypoint = waypoint;
            }

            if (maxWaypoint == null)
                return null;
            
            const double offset = 0.01;

            var lowerLat = maxWaypoint.Latitude - offset;
            var lowerLon = maxWaypoint.Longitude - offset;

            var upperLat = maxWaypoint.Latitude + offset;
            var upperLon = maxWaypoint.Longitude + offset;

            return _context.Summits.FirstOrDefaultAsync(x => x.Latitude > lowerLat && x.Latitude < upperLat
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