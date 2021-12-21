namespace SummitDiary.Core.Models.Common;

public class Bounds
{
    public double NeLat { get; }
    public double NeLon { get; }
    public double SwLat { get; }
    public double SwLon { get; }

    public Bounds(double neLat, double neLon, double swLat, double swLon)
    {
        NeLat = neLat;
        NeLon = neLon;
        SwLat = swLat;
        SwLon = swLon;
    }
}