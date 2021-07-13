using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Endpoints.Stats.Dto;
using SummitDiary.Core.Endpoints.Stats.Queries;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Web.Api
{
    public class StatsController : BaseApiController
    {
        [HttpGet("highestClimbed")]
        public async Task<ActionResult<SummitDto>> GetHighestClimbedSummit()
        {
            return await Mediator.Send(new GetHighestClimbedSummitQuery());
        }

        [HttpGet("timeline")]
        public async Task<ActionResult<List<TimelineStatDto>>> GetTimeline([FromQuery] GetActivityTimelineQuery query)
        {
            return await Mediator.Send(query);
        }
        [HttpGet("country")]
        public async Task<ActionResult<List<BaseStatDto>>> GetCountryStats([FromQuery] GetCountryStatsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("totals")]
        public async Task<ActionResult<TotalsDto>> GetTotals()
        {
            return await Mediator.Send(new GetTotalsQuery());
        }
    }
}