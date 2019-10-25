using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.AspNetCore.Filters
{
    public class ExecutionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TODO: Before 

            await next();
            
            //TODO: After
            
        }
    }
}
