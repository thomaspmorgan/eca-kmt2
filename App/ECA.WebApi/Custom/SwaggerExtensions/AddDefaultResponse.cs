using System.Web.Http.Description;
using Swashbuckle.Swagger;
using System.Web.Http;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class AddDefaultResponse : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var errorSchema = schemaRegistry.GetOrRegister(typeof(HttpError));

            operation.responses.Add("default", new Response
                {
                    description = "Error",
                    schema = errorSchema
                });
        }
    }
}
