using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NetTopologySuite.IO;
using SummitDiary.Core.Endpoints.Gpx.Commands;
using SummitDiary.Core.Endpoints.Gpx.Dto;

namespace SummitDiary.Core.Services
{
    public class GpxAnalyzer
    {
        public AnalysisResultDto AnalyzeGpx(Stream gpxFile, int compressValue = 1)
        {
            using var xmlReader = new XmlTextReader(gpxFile);
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

            var path = new List<Waypoint>();
            
            for (var i = 0; i < waypoints.Count - 1; i+=compressValue)
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
                path.Add(new Waypoint(current.Latitude, current.Longitude, current.ElevationInMeters ?? 0, current.TimestampUtc??default));
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
                StartPoint = new Waypoint(startPoint.Latitude, startPoint.Longitude, startPoint.ElevationInMeters ?? 0, startPoint.TimestampUtc ?? default),
                EndPoint = new Waypoint(endPoint.Latitude, endPoint.Longitude, endPoint.ElevationInMeters ?? 0, endPoint.TimestampUtc ?? default),
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

    }
}