using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace PonyUrl.Common
{
    public static class HttpRequestExtensions
    {
        static readonly string[] usingRoutes = { "index", "swagger", "api", "error", "notfound" };

        public static bool TryGetSlug(this HttpRequest request, out string slug)
        {
            var isSlug = request.Path.HasValue && (request.Path.Value != "/" && !usingRoutes.Any(s => request.Path.Value == $"/{s}"));

            slug = string.Empty;

            if (isSlug)
            {
                slug = request.Path.Value.TrimStart('/');
            }

            return isSlug;
        }

        public static T GetHeaderValue<T>(this HttpRequest request, string key)
        {
            try
            {

                if (request.Headers.TryGetValue(key, out StringValues values))
                {
                    return (T)Convert.ChangeType(values.FirstOrDefault(), typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetIPAddress(this HttpRequest request)
        {
            return Convert.ToString(request.HttpContext.Connection.RemoteIpAddress);
        }
        public static string GetReferer(this HttpRequest request)
        {
            return request.GetHeaderValue<string>("Referer");
        }

        public static string GetUserAgent(this HttpRequest request)
        {
            return request.GetHeaderValue<string>("User-Agent");
        }
    }
}
