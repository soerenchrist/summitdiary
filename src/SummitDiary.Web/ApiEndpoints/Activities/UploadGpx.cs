using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.ActivityAggregate.Specs;

namespace SummitDiary.Web.ApiEndpoints.Activities;

public class UploadGpx : EndpointBaseAsync
    .WithRequest<UploadGpxRequest>
    .WithoutResult
{
    private readonly IReadRepository<Activity> _activityRepository;
    private readonly IRepository<Attachment> _attachmentRepository;
    private readonly IConfiguration _configuration;

    public UploadGpx(IReadRepository<Activity> activityRepository,
        IRepository<Attachment> attachmentRepository,
        IConfiguration configuration)
    {
        _activityRepository = activityRepository;
        _attachmentRepository = attachmentRepository;
        _configuration = configuration;
    }
    
    [HttpPost("/api/activities/{activityId}/gpx")]
    public override async Task<ActionResult> HandleAsync([FromRoute] UploadGpxRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var activity =
            await _activityRepository.GetBySpecAsync(new GetActivityByIdSpec(request.ActivityId));

        if (activity == null || request.File == null)
            return NotFound();
            
        var basePath = _configuration.GetValue("Files:Gpx:Path", "files/gpx");
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);
            
        var originalFilename = request.File.FileName;

        var newFilename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".gpx";
        var path = Path.Combine(basePath, newFilename);

        var fileStream = System.IO.File.Create(path);
        await request.File.CopyToAsync(fileStream, cancellationToken);
        fileStream.Close();

        var attachment = new Attachment
        {
            ActivityId = request.ActivityId,
            FileName = originalFilename,
            FilePath = path,
            FileType = FileType.Gpx
        };
        await _attachmentRepository.AddAsync(attachment, cancellationToken);
        return Ok();
    }
}