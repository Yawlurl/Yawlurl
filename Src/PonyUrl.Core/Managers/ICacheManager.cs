using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PonyUrl.Core
{
    public interface ICacheManager
    {
        Task<T> Get<T>(string key);

        Task<bool> Set<T>(string key, T value);

        Task<bool> Delete(string key);

        Task<bool> IsExist(string key);

        Task<string> GetUrl(string shortKey);

        Task<bool> SetUrl(string shortKey, string url);

        Task<bool> IsExistUrl(string shortKey);

        Task Clear();

        Task<bool> IsConnected();

        Task<bool> ClearUrls();

        Task<bool> IsExistUrls();

        Task<bool> DeleteUrl(string shortKey);
    }
}
