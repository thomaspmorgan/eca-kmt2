﻿using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class AddFileUploadParams : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "FileUpload_PostFormData")
            {
                operation.consumes.Add("application/form-data");
                operation.parameters = new[]
                {
                    new Parameter
                    {
                        name = "file",
                        @in = "formData",
                        required = true,
                        type = "file"
                    }
                };
            }
        }
    }
}
