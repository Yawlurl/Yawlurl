using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace PonyUrl.Infrastructure.Redis.Cache
{
    public class RedisCacheProvider
    {
        public string Host { get; set; }
        public int DefaultDb { get; set; }
        public TimeSpan DefaultCacheDuration => TimeSpan.FromSeconds(30);

        public RedisCacheProvider(string host)
        {
            Host = host;
        }

        public ConnectionMultiplexer GetConnection()
        {
            return ConnectionMultiplexer.Connect(Host);
        }

        public IDatabase GetDatabase()
        {
            return GetConnection().GetDatabase(DefaultDb);
        }

        public IDatabase Instance
        {
            get
            {
                return GetDatabase();
            }
        }

        public T Get<T>(string key)
        {
            if (Instance.KeyExists(key))
            {
                var redisValue = Instance.StringGet(key);

                if (redisValue.HasValue)
                    return JsonConvert.DeserializeObject<T>(redisValue);
            }

            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            if (value == null)
            {
                return;
            }

            var jsonValue = JsonConvert.SerializeObject(value);

            Instance.StringSet(key, jsonValue, DefaultCacheDuration);

        }

        public void Set<T>(string key, T value, TimeSpan cacheTime)
        {
            if (value == null)
            {
                return;
            }

            var jsonValue = JsonConvert.SerializeObject(value);

            Instance.StringSet(key, jsonValue, cacheTime);

        }

        public void Delete(string key)
        {
            if (Instance.KeyExists(key))
            {
                Instance.KeyDelete(key);
            }
        }
    }
}
