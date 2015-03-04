using ECA.Core.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A ModelNotFoundExceptionFilter is used when a ModelNotFoundException is unhandled.
    /// </summary>
    public class ModelNotFoundExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ModelNotFoundException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}