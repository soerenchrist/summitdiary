using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SummitDiary.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator 
            ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}