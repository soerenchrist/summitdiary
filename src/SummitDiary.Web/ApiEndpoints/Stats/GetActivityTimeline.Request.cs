namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetActivityTimelineRequest
{
    public string ValueType { get; set; } = "elevation";
    public string TimeType { get; set; } = "year";
}