namespace SummitDiary.Web.ApiEndpoints.Analyze;

public class ExportGpxRequest
{
    public List<PathPointDto> Points { get; set; } = new();
}