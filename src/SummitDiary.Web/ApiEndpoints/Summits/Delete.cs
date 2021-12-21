using SummitDiary.Core.Models.SummitAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class Delete : BaseAsyncEndpoint
    .WithRequest<DeleteSummitRequest>
    .WithoutResponse
{
    private readonly IRepository<Summit> _summitRepository;

    public Delete(IRepository<Summit> summitRepository)
    {
        _summitRepository = summitRepository;
    }
    
    [HttpDelete("/api/summits/{id:int}")]
    [SwaggerOperation(
        Summary = "Deletes a summit",
        Description = "Deletes the summit with the given id",
        OperationId = "Summit.Delete",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteSummitRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var summit = await _summitRepository.GetByIdAsync(request.SummitId, cancellationToken);
        if (summit == null)
            return NotFound();

        await _summitRepository.DeleteAsync(summit, cancellationToken);

        return Ok();
    }
}