using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SummitDiary.Core.Common.Exceptions;

namespace SummitDiary.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                context.Result = new JsonResult(context.Exception.Message);
                
                return;
            }
            base.OnException(context);
        }
    }
}