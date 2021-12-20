using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Gpx.Dto
{
    public class AnalysisResultDto
    {
        public string ProposedTitle { get; set; } = string.Empty;
        public SummitDto? ProposedSummit { get; set; }
        public int ElevationUp { get; set; }
        public int ElevationDown { get; set; }
        public DateTime? HikeDate { get; set; }
        public double Distance { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public List<Waypoint> Path { get; set; } = new();
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