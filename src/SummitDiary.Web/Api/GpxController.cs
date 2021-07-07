using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Endpoints.Gpx.Commands;
using SummitDiary.Core.Endpoints.Gpx.Dto;

namespace SummitDiary.Web.Api
{
    public class GpxController : BaseApiController
    {
        [HttpPost("analyze")]
        public async Task<ActionResult<AnalysisResultDto>> AnalyzeGpx([FromForm] AnalyzeGpxCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}