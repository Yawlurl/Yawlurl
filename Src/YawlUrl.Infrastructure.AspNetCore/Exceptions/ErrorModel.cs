using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YawlUrl.Infrastructure.AspNetCore.Exceptions
{
    public class ErrorModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
