using System;
using System.Collections.Generic;
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
        public List<Coordinate> Path { get; set; }
        public Coordinate StartPoint { get; set; }
        public Coordinate EndPoint { get; set; }
    }

    public struct Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }

        public Coordinate(double latitude, double longitude, double elevation)
        {
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
        }
    }
}