using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        
        [HttpPost("analyzepath")]
        public async Task<ActionResult<AnalysisResultDto>> AnalyzePath([FromBody] AnalyzePathCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("generateGpx")]
        public async Task<ActionResult<IFormFile>> GenerateGpx([FromBody] ExportGpxCommand command)
        {
            var result = await Mediator.Send(command);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}