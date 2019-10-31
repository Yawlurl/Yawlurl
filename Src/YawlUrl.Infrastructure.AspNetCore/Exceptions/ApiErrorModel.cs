using Newtonsoft.Json;
using System.Collections.Generic;

namespace YawlUrl.Infrastructure.AspNetCore.Exceptions
{
    public class ApiErrorModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("is_error")]
        public bool IsError { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("errors")]
        public List<ErrorModel> Errors { get; set; }

        public ApiErrorModel(string message)
        {
            Message = message;
            IsError = true;
        }

        //TODO:Validation

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
