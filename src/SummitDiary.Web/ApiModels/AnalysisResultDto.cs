using SummitDiary.Core.Common.Mapping;
using SummitDiary.Core.Models.Common;

namespace SummitDiary.Web.ApiModels;

public class AnalysisResultDto : IMapFrom<AnalysisResult>
{
    public string ProposedTitle { get; set; } = string.Empty;
    public SummitDto? ProposedSummit { get; set; }
    public int ElevationUp { get; set; }
    public int ElevationDown { get; set; }
    public DateTime? HikeDate { get; set; }
    public double Distance { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public List<WaypointDto> Path { get; set; } = new();
    public WaypointDto StartPoint { get; set; }
    public WaypointDto EndPoint { get; set; }
}