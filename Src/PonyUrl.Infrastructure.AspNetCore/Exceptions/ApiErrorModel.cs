using FluentValidation;
using Newtonsoft.Json;
using PonyUrl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore.Exceptions
{
    public class ApiErrorModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("is_error")]
        public bool IsError { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        public ApiErrorModel(string message)
        {
            Message = message;
            IsError = true;
        }

        //TODO:Validation
    }


}
