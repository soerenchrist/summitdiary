using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Endpoints.Regions.Dto;
using SummitDiary.Core.Endpoints.Regions.Queries;

namespace SummitDiary.Web.Api
{
    public class RegionsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<RegionDto>>> GetRegions()
        {
            return await Mediator.Send(new GetRegionsQuery());
        }
    }
}