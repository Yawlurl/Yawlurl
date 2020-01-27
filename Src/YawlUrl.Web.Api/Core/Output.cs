using Microsoft.AspNetCore.Mvc;
using YawlUrl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace YawlUrl.Web.Api.Core
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

        public Output<TData> AddTrace(string traceId)
        {
            AddMetadata("traceId", traceId);

            return this;
        }

        public Output<TData> AddConsumer(string consumer)
        {
            AddMetadata("consumer", consumer);
            
            return this;
        }
        
        public Output<TData> AddActivity(string requestId)
        {
            AddMetadata("requestId", requestId);
            return this;
        }
    }
}
