using System.Web.Http.ExceptionHandling;
using log4net;

namespace PwTransferApp.Controllers.Filters
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(UnhandledExceptionLogger));

        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error($"unhandled exception in {context.Request.RequestUri} {context.Request.Method}", context.Exception);
        }
    }
}