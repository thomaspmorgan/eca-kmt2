using ECA.Core.Exceptions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ConcurrencyExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// The reason failed message.
        /// </summary>
        public const string CONCURRENCY_EXCEPTION_REASON_FAILED_MESSAGE = "Concurrency Error.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is EcaDbUpdateConcurrencyException)
            {
                var exception = (EcaDbUpdateConcurrencyException)context.Exception;
                var concurrentEntities = exception.ConcurrentEntities.ToList();
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = CONCURRENCY_EXCEPTION_REASON_FAILED_MESSAGE
                };
            }
        }
    }
}