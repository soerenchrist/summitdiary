namespace SummitDiary.Core.Models.Common;

public class Waypoint
{
    public double Latitude { get; }
    public double Longitude { get; }
    public double Elevation { get; }
    public DateTime DateTime { get; }

    public Waypoint(double latitude, double longitude, double elevation, DateTime dateTime)
    {
        Latitude = latitude;
        Longitude = longitude;
        Elevation = elevation;
        DateTime = dateTime.ToLocalTime();
    }
}