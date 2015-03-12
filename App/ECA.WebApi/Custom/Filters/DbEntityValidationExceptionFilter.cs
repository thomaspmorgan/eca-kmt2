using ECA.Core.Exceptions;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace ECA.WebApi.Custom.Filters
{
    /// <summary>
    /// A DbEntityValidationExceptionFilter is used when a DbEntityValidationException is unhandled.
    /// </summary>
    public class DbEntityValidationExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is DbEntityValidationException)
            {
                var exception = (DbEntityValidationException)context.Exception;
                var entityValidationErrors = exception.EntityValidationErrors;
                var validationErrors = entityValidationErrors.SelectMany(x => x.ValidationErrors).ToList();
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<ValidationErrorResponseContent>(
                        new ValidationErrorResponseContent(ValidationErrorResponseContent.VALIDATION_FAILED_ERROR_MESSAGE, validationErrors), 
                        new JsonMediaTypeFormatter()),
                    ReasonPhrase = ValidationErrorResponseContent.VALIDATION_FAILED_REASON_FAILED_MESSAGE
                };
            }
        }
    }

}