using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyUrl.Web.Api.Filters
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "consumerCode",
                In = "header",
                Type = "string",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "traceId",
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}
