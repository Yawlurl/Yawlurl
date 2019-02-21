using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

        public ApiException(string message, int statusCode = 500, List<string> errors = null) : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
