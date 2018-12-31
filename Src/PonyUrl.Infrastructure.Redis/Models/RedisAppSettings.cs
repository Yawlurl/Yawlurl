using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Infrastructure.Redis.Models
{
    public class RedisAppSettings
    {
        public string Host { get; set; }
        public int DefaultDb { get; set; }
    }
}
