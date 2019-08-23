using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.AspNetCore.Authorization
{
    public class JwtTokenModel
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("refresh_token")]
        public string  RefreshToken { get; set; }

        [JsonProperty("expires")]
        public DateTime ExpireDateTime { get; set; }

    }
}
