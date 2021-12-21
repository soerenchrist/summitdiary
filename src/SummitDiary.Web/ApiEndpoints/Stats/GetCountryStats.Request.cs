namespace SummitDiary.Web.ApiEndpoints.Stats;

public class GetCountryStatsRequest
{
    [FromQuery] public string ValueType { get; set; } = "elevation";
}