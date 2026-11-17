using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innolux.Api
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();
            //MemberAuthorizeAttribute 自定义的身份验证特性标记
            var isAuthor = operation != null && context != null;
            bool showcheckToken = false;
            var descriptor = context.ApiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            descriptor.ControllerTypeInfo.CustomAttributes.ToList().ForEach(item =>
            {
                var one = item.ConstructorArguments.ToList().Find(a => a.Value.ToString() == "checkToken");
                if (one.Value != null)
                {
                    showcheckToken = true;
                }
            });

            var tokenAttributes = context.MethodInfo.CustomAttributes.ToList();
            tokenAttributes.ForEach(item =>
            {
                var one = item.ConstructorArguments.ToList().Find(a => a.Value.ToString() == "checkToken");
                if (one.Value != null)
                {
                    showcheckToken = true;
                }
            });
            if (isAuthor && showcheckToken)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "token",
                    In = ParameterLocation.Header,
                    Description = "",
                    Schema = new OpenApiSchema { Type = "string" },
                    Required = false //是否必选
                });
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "emp_id",
                    In = ParameterLocation.Header,
                    Description = "",
                    Schema = new OpenApiSchema { Type = "string" },
                    Required = false //是否必选
                });
            }
        }
    }
}
