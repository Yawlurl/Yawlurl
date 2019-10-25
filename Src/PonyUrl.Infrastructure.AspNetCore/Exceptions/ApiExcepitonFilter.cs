using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PonyUrl.Domain;
using PonyUrl.Infrastructure.AspNetCore.Exceptions;
using Serilog;
using Serilog.Core;
using System;

namespace PonyUrl.Infrastructure.AspNetCore
{
    /// <summary>
    /// Handle all exceptions and return friendly model
    /// </summary>
    public class ApiExcepitonFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            ApiErrorModel apiErrorModel = null;
            
            
            if (context.Exception is ApiException)
            {
                var ex = context.Exception as ApiException;

                apiErrorModel = new ApiErrorModel(ex.Message)
                {
                    Errors = ex.Errors,
                };
                context.HttpContext.Response.StatusCode = ex.StatusCode;

            }
            else if (context.Exception is DomainException || context.Exception is ApplicationException)
            {
                apiErrorModel = new ApiErrorModel(context.Exception.Message);

                context.HttpContext.Response.StatusCode = 403;   
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
#endif
                apiErrorModel = new ApiErrorModel(msg)
                {
                    Detail = stack,
                };

                context.HttpContext.Response.StatusCode = 500;
            }

            //Set Exception Type
            apiErrorModel.Type = context?.Exception?.GetType().Name;

            context.Result = new JsonResult(apiErrorModel);

            Log.Error(context.Exception, "");

            base.OnException(context);
        }
    }
}
