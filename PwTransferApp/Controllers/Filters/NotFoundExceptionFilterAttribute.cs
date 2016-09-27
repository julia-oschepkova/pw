using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Common.Exceptions;

namespace PwTransferApp.Controllers.Filters
{
    public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is EntityNotFoundException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}