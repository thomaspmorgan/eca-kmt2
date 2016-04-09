using ECA.Business.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class EcaBusinessExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is EcaBusinessException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Eca Business Exception"
                };
            }
        }
    }
}