namespace SummitDiary.Web.ApiEndpoints.Summits;

public class DeleteSummitRequest
{
    [FromRoute] public int SummitId { get; set; }
}