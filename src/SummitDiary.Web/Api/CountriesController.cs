using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Countries.Dto;
using SummitDiary.Core.Endpoints.Countries.Queries;

namespace SummitDiary.Web.Api
{
    public class CountriesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<CountryDto>>> GetCountries()
        {
            return await Mediator.Send(new GetCountriesQuery());
        }
    }
}