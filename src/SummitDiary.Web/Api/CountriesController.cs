using Microsoft.AspNetCore.Mvc;
using SummitDiary.Core.Endpoints.Countries.Commands;
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

        [HttpPost]
        public async Task<ActionResult<CountryDto>> CreateCountry([FromBody] CreateCountryCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}