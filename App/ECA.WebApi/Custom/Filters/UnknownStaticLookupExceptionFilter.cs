using ECA.Core.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A UnknownStaticLookupExceptionFilter is used when a static lookup exception is unhandled.
    /// </summary>
    public class UnknownStaticLookupExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is UnknownStaticLookupException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Invalid Lookup Value"
                
                };
            }
        }
    }
}