using System;
using System.Collections.Generic;
using System.Text;

namespace PonyUrl.Integration.Test.Redis
{
    [Serializable]
    public class RedisTestModel
    {
        public string Url { get; set; }

        public RedisTestModel()
        {

        }

        public RedisTestModel(string url) : this()
        {
            Url = url;
        }
    }
}
