using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyDocumentVendorExtensions : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.vendorExtensions.Add("x-document", "foo");
        }
    }
}
