namespace SummitDiary.Web.ApiEndpoints.Activities;

public class ListActivitiesPaginatedRequest
{
    [FromQuery] public string? SearchText { get; set; }
    [FromQuery] public string? SortBy { get; set; }
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
    [FromQuery] public int? SummitId { get; set; }
    [FromQuery] public bool SortDescending { get; set; } = true;
}