using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace PwTransferApp.Controllers
{
    [Authorize]
    public abstract class LoggedInController : ApiController
    {
        protected Guid UserId => Guid.Parse(RequestContext.Principal.Identity.GetUserId());
    }
}