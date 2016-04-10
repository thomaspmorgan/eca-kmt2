using System;
using Swashbuckle.Swagger;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplySchemaVendorExtensions : ISchemaFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            schema.vendorExtensions.Add("x-schema", "bar");
        }
    }
}
