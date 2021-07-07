using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MediatR;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.IO;
using SummitDiary.Core.Endpoints.Gpx.Dto;

namespace SummitDiary.Core.Endpoints.Gpx.Commands
{
    public class AnalyzeGpxCommand : IRequest<AnalysisResultDto>
    {
        public IFormFile GpxFile { get; set; }
    }

    public class AnalyzeGpxCommandHandler : IRequestHandler<AnalyzeGpxCommand, AnalysisResultDto>
    {
        public async Task<AnalysisResultDto> Handle(AnalyzeGpxCommand request, CancellationToken cancellationToken)
        {
            using var xmlReader = new XmlTextReader(request.GpxFile.OpenReadStream());
            var file = GpxFile.ReadFrom(xmlReader, new GpxReaderSettings());

            double totalElevationUp = 0.0;
            double totalElevationDown = 0.0;
            double totalDistance = 0.0;

            for (var i = 0; i < file.Waypoints.Count - 1; i++)
            {
                var current = file.Waypoints[i];
                var next = file.Waypoints[i + 1];

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
            }

            return new AnalysisResultDto
            {
                Distance = totalDistance,
                ElevationDown = (int) totalElevationDown,
                ElevationUp = (int) totalElevationUp,
                StartTime = file.Waypoints.First().TimestampUtc,
                EndTime = file.Waypoints.Last().TimestampUtc,
            };
        }
    }
    
    static class DistanceAlgorithm
    {
        const double RADIUS = 6371;

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
            return angle * RADIUS;
        }

    }
    
}