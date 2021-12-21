namespace SummitDiary.Web.ApiEndpoints.Activities;

public class UploadGpxRequest
{
    [FromForm] public IFormFile? File { get; set; }
    [FromRoute] public int ActivityId { get; set; }
}