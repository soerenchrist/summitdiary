using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Common.Models.Common;
using SummitDiary.Core.Endpoints.Activities.Commands;
using SummitDiary.Core.Endpoints.Activities.Dto;
using SummitDiary.Core.Endpoints.Activities.Query;

namespace SummitDiary.Web.Api
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ActivityDto>>> GetActivitiesPaginated(
            [FromQuery] GetActivitiesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityDto>> GetActivityById([FromRoute] int id)
        {
            return await Mediator.Send(new GetActivityByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("{id:int}/gpx")]
        public async Task<ActionResult> UploadGpx([FromRoute]int id, [FromForm] UploadGpxCommand command)
        {
            command.ActivityId = id;
            await Mediator.Send(command);
            return Ok();
        }
    }
}