using ECA.WebApi.Security;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;

namespace ECA.WebApi.Custom.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class AddResourceAuthorizePermissionRequirement : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            var attributes = apiDescription.ActionDescriptor.GetCustomAttributes<ResourceAuthorizeAttribute>();
            var permissions = new List<string>();
            foreach (var attribute in attributes)
            {
                var permissionString = String.Format("Requires <strong>{0}</strong> permission on <strong>{1}</strong> resource type.", 
                    attribute.Permission.PermissionName, 
                    attribute.Permission.ResourceType);
                operation.description += permissionString;
            }
        }
    }
}