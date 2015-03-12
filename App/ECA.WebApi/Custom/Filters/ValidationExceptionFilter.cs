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
                var validationResults = validationException.ValidationResults;
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<ValidationErrorResponseContent>(
                        new ValidationErrorResponseContent(ValidationErrorResponseContent.VALIDATION_FAILED_ERROR_MESSAGE, validationResults), 
                        new JsonMediaTypeFormatter()),
                    ReasonPhrase = ValidationErrorResponseContent.VALIDATION_FAILED_REASON_FAILED_MESSAGE
                };
            }
        }
    }
}