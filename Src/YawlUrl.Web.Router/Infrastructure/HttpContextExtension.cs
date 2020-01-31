using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawlUrl.Web.Router
{
    public static class HttpContextExtension
    {
        public static bool TryGetSlug(this HttpContext httpContext,out string slug)
        {
            bool result = true;
            try
            {
                slug = httpContext.Request.Path.HasValue && 
                       httpContext.Request.Path.Value.TrimStart('/').Length > 0 ? httpContext.Request.Path.Value.TrimStart('/') 
                                                                                : string.Empty;

                if (string.IsNullOrEmpty(slug))
                    throw new ArgumentNullException(nameof(slug));

                result = true;
            }
            catch
            {
                result = false;
                slug = string.Empty;
            }
            return result;
        }
    }
}
