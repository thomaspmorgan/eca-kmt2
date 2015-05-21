using ECA.WebApi.Security;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Custom.SwaggerExtensions
{
    public class AddResourceAuthorizePermissionRequirement : IOperationFilter
    {
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