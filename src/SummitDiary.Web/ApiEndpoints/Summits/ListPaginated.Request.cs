namespace SummitDiary.Web.ApiEndpoints.Summits;

public class ListSummitsPaginatedRequest
{
    [FromQuery] public string SearchText { get; set; } = "";
    [FromQuery] public string? SortBy { get; set; }
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
    [FromQuery] public bool OnlyClimbed { get; set; } = false;
    [FromQuery] public bool SortDescending { get; set; }
    [FromQuery] public double? NeLat { get; set; }
    [FromQuery] public double? NeLon { get; set; }
    [FromQuery] public double? SwLat { get; set; }
    [FromQuery] public double? SwLon { get; set; }
}