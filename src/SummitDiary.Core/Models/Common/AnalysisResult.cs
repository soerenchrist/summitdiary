using SummitDiary.Core.Models.SummitAggregate;

namespace SummitDiary.Core.Models.Common;

public class AnalysisResult
{
    public string ProposedTitle { get; set; } = string.Empty;
    public Summit? ProposedSummit { get; set; }
    public int ElevationUp { get; set; }
    public int ElevationDown { get; set; }
    public DateTime? HikeDate { get; set; }
    public double Distance { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public List<Waypoint> Path { get; set; } = new();
    public Waypoint? StartPoint { get; set; }
    public Waypoint? EndPoint { get; set; }
}