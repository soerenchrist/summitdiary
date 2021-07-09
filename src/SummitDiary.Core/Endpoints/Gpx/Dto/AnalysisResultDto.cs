using System;
using System.Collections.Generic;
using Bogus.DataSets;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Gpx.Dto
{
    public class AnalysisResultDto
    {
        public string ProposedTitle { get; set; }
        public Summit ProposedSummit { get; set; }
        public int ElevationUp { get; set; }
        public int ElevationDown { get; set; }
        public DateTime? HikeDate { get; set; }
        public double Distance { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<Waypoint> Path { get; set; }
        public Waypoint StartPoint { get; set; }
        public Waypoint EndPoint { get; set; }
    }

    public struct Waypoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime DateTime { get; set; }

        public Waypoint(double latitude, double longitude, double elevation, DateTime dateTime)
        {
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
            DateTime = dateTime.ToLocalTime();
        }
    }
}