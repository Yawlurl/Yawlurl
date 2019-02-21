using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PonyUrl.Infrastructure.AspNetCore.Exceptions;
using System;
using System.Net.Http;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore
{
    public class ApiExcepitonFilterAttribute : ExceptionFilterAttribute
    {

    
        public override void OnException(ExceptionContext context)
        {
            ApiErrorModel apiErrorModel = null;


            if (context.Exception is ApiException)
            {
                var ex = context.Exception as ApiException;

                context.Exception = null;

                apiErrorModel = new ApiErrorModel(ex.Message)
                {
                    Errors = ex.Errors
                };

                context.HttpContext.Response.StatusCode = ex.StatusCode;

            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiErrorModel = new ApiErrorModel("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
            }
            else
            {
#if !DEBUG
                var msg = "An unhandled error occurred.";                
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;

                apiErrorModel = new ApiErrorModel(msg)
                {
                    Detail = stack
                };

                context.HttpContext.Response.StatusCode = 500;
#endif
            }


            context.Result = new JsonResult(apiErrorModel);

            base.OnException(context);
        }
    }
}
