using Microsoft.AspNetCore.Mvc;
using PonyUrl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Web.Api.Core
{
    [Serializable]
    public class Output<TData>
    {
        public TData Data { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
       
        public Output(TData data)
        {
            Data = data;

            Metadata = new Dictionary<string, object>();

            AddMetadata("responseDateUtc", DateTime.UtcNow);
        }

        public Output<TData> AddMetadata(string key, object value)
        {
            Metadata.TryAdd(key, value);

            return this;
        }

        public Output<TData> AddTraceId(string traceId)
        {
            AddMetadata("traceId", traceId);

            return this;
        }

        public Output<TData> AddConsumerCode(string consumerCode)
        {
            AddMetadata("consumerCode", consumerCode);
            
            return this;
        }   
    }
}
