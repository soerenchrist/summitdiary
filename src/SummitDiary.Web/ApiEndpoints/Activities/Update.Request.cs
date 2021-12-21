namespace SummitDiary.Web.ApiEndpoints.Activities;

public class UpdateActivityRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime HikeDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int Rating { get; set; }
    public double ElevationUp { get; set; }
    public double ElevationDown { get; set; }
    public double Distance { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int Duration { get; set; }
    public List<int> SummitIds { get; set; } = new();
}