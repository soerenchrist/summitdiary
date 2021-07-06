using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Common.Models.Common;
using SummitDiary.Core.Endpoints.Diary.Dto;
using SummitDiary.Core.Endpoints.Diary.Query;

namespace SummitDiary.Web.Api
{
    public class DiaryController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DiaryEntryDto>>> GetDiaryEntriesPaginated(
            [FromQuery] GetDiaryEntriesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}