using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Common.Models.Common;
using SummitDiary.Core.Endpoints.Summits.Commands;
using SummitDiary.Core.Endpoints.Summits.Dto;
using SummitDiary.Core.Endpoints.Summits.Queries;

namespace SummitDiary.Web.Api
{
    public class SummitsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<SummitDto>>> GetSummitsWithPagination(
            [FromQuery] GetSummitsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{summitId:int}")]
        public async Task<ActionResult<SummitDto>> GetSummitById([FromRoute] int summitId)
        {
            return await Mediator.Send(new GetSummitByIdQuery(summitId));
        }

        [HttpPost]
        public async Task<ActionResult<SummitDto>> CreateSummit([FromBody] CreateSummitCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{summitId:int}")]
        public async Task<ActionResult> DeleteSummit(int summitId)
        {
            await Mediator.Send(new DeleteSummitCommand(summitId));
            return Ok();
        } 
    }
}