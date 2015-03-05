using ECA.Business.Exceptions;
using ECA.Business.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A ModelNotFoundExceptionFilter is used when a ModelNotFoundException is unhandled.
    /// </summary>
    public class ValidationExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
            {
                var validationException = context.Exception as ValidationException;
                var validationResults = validationException.ValidationResults.ToList();
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<List<BusinessValidationResult>>(validationResults, new JsonMediaTypeFormatter()),
                    ReasonPhrase = "Entity validation failed."

                };
            }
        }
    }
}