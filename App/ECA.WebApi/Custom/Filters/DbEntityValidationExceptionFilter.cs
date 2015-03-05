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
                var errorMessages = validationErrors.Select(x => x.ErrorMessage).ToList();

                var content = new EntityValidationErrorResponseContent
                {
                    Message = exception.Message,
                    ValidationErrors = errorMessages
                };
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<EntityValidationErrorResponseContent>(content, new JsonMediaTypeFormatter()),
                    ReasonPhrase = "Entity validation failed."

                };
            }
        }
    }

    public class EntityValidationErrorResponseContent
    {
        public string Message { get; set; }

        public IEnumerable<string> ValidationErrors { get; set; }
    }
}