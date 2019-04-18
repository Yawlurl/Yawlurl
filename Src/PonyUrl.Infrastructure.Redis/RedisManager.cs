using Newtonsoft.Json;
using PonyUrl.Common;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.Redis
{
    public class RedisManager
    {
        public string Host { get; set; }
        public int Database { get; set; }
        public int Port { get; set; }

        public TimeSpan DefaultCacheDuration => TimeSpan.FromSeconds(30);
        private const string ShortUrlsKey = "shortuls";
        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public RedisManager(string host, int database = 0, int port = 6379)
        {
            Check.ArgumentNotNullOrEmpty(host);

            Host = host;
            Database = database;
            Port = port;

            lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect(Host); });
        }

        public bool IsConnected { get { return GetConnection().IsConnected; } }


        public ConnectionMultiplexer GetConnection()
        {
            return lazyConnection.Value;
        }

        public IDatabase GetDatabase()
        {
            return GetConnection().GetDatabase(Database);
        }

        public IDatabase DbInstance
        {
            get
            {
                return GetDatabase();
            }
        }
        /// <summary>
        /// Drop that database
        /// </summary>
        /// <returns></returns>
        public async Task FlushDb()
        {
            await GetConnection().GetServer(Host, Port).FlushDatabaseAsync(Database, CommandFlags.PreferMaster);
        }

        /// <summary>
        /// Get value by key
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="key">Unique Identifier</param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            var result = default(T);

            if (Check.IsNullOrEmpty(key))
            {
                return result;
            }

            if (await DbInstance.KeyExistsAsync(key))
            {
                var redisValue = await DbInstance.StringGetAsync(key, CommandFlags.PreferSlave);

                if (redisValue.HasValue)
                {
                    result = JsonConvert.DeserializeObject<T>(redisValue);
                }
            }

            return result;
        }
        /// <summary>
        /// Add or update value by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> Set<T>(string key, T value)
        {
            if (Check.IsNullOrEmpty(key))
            {
                return false;
            }

            var jsonValue = JsonConvert.SerializeObject(value);

            return await DbInstance.SetAddAsync(key, jsonValue, CommandFlags.PreferMaster);
        }
        /// <summary>
        /// Delete item by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string key)
        {
            if (Check.IsNullOrEmpty(key) || !DbInstance.KeyExists(key)) return false;

            return await DbInstance.KeyDeleteAsync(key, CommandFlags.PreferMaster);

        }
        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SetHashKey(string key, string field, string value)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return false;

            return await DbInstance.HashSetAsync(key, field, value, When.NotExists, CommandFlags.PreferMaster);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<string> GetHashKeyValue(string key, string field)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return string.Empty;

            return await DbInstance.HashGetAsync(key, field, CommandFlags.PreferSlave);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<bool> IsExistHashKeyField(string key, string field)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return false;
            
            return await DbInstance.HashExistsAsync(key, field, CommandFlags.PreferSlave);
        }
    }
}
