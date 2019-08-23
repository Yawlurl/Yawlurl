using Newtonsoft.Json;
using PonyUrl.Common;
using PonyUrl.Core;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.Redis
{
    public class RedisManager : ICacheManager
    {
        public string Host { get; private set; }
        public int Database { get; private set; }
        public int Port { get; private set; }

        public TimeSpan DefaultCacheDuration => TimeSpan.FromSeconds(30);
        private const string ShortUrlsKey = "short_urls";
        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public RedisManager(string host, int database = 0, int port = 6379)
        {
            Check.ArgumentNotNullOrEmpty(host);

            Host = host;
            Database = database;
            Port = port;

            lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect(Host); });
        }

        #region Private Methods
        private ConnectionMultiplexer GetConnection()
        {
            return lazyConnection.Value;
        }

        private IDatabase GetDatabase()
        {
            return GetConnection().GetDatabase(Database);
        }

        private IDatabase DbInstance
        {
            get
            {
                return GetDatabase();
            }
        }

        private async Task FlushDb()
        {
            await GetConnection().GetServer(Host, Port).FlushDatabaseAsync(Database, CommandFlags.PreferMaster);
        }
        private async Task<bool> SetHashKey(string key, string field, string value)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return false;

            return await DbInstance.HashSetAsync(key, field, value, When.Always, CommandFlags.PreferMaster);
        }

        private async Task<string> GetHashKeyValue(string key, string field)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return string.Empty;

            return await DbInstance.HashGetAsync(key, field, CommandFlags.PreferSlave);
        }

        private async Task<bool> IsExistHashKeyField(string key, string field)
        {
            if (Check.IsNullOrEmpty(key) || Check.IsNullOrEmpty(field)) return false;

            return await DbInstance.HashExistsAsync(key, field, CommandFlags.PreferSlave);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
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
                var redisValue = await DbInstance.StringGetAsync(key, CommandFlags.PreferMaster);

                if (redisValue.HasValue)
                {
                    result = JsonConvert.DeserializeObject<T>(redisValue);
                }
            }

            return result;
        }
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string key)
        {
            if (Check.IsNullOrEmpty(key) || !DbInstance.KeyExists(key)) return false;

            return await DbInstance.KeyDeleteAsync(key, CommandFlags.PreferMaster);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> IsExist(string key)
        {
            Check.ArgumentNotNullOrEmpty(key);

            return await DbInstance.KeyExistsAsync(key, CommandFlags.PreferMaster);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortKey"></param>
        /// <returns></returns>
        public async Task<string> GetUrl(string shortKey)
        {
            return await GetHashKeyValue(ShortUrlsKey, shortKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortKey"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> SetUrl(string shortKey, string url)
        {
            return await SetHashKey(ShortUrlsKey, shortKey, url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortKey"></param>
        /// <returns></returns>
        public async Task<bool> IsExistUrl(string shortKey)
        {
            return await IsExistHashKeyField(ShortUrlsKey, shortKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await FlushDb();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsConnected()
        {
            return await Task.FromResult(GetConnection().IsConnected);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ClearUrls()
        {
            return await Delete(ShortUrlsKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsExistUrls()
        {
            return await DbInstance.KeyExistsAsync(ShortUrlsKey, CommandFlags.PreferSlave);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortKey"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUrl(string shortKey)
        {
            return await DbInstance.HashDeleteAsync(ShortUrlsKey, shortKey, CommandFlags.PreferMaster);
        }
    }
}
