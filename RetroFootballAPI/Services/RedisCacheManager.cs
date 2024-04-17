using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RetroFootballAPI.Repositories;
using StackExchange.Redis;

namespace RetroFootballAPI.Services
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<string> GetRedisCache(string key)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(key);
            return cacheResponse;
        }

        public async Task SetRedisCache(string key, object response, TimeSpan timeOut)
        {
            if (response == null) return;

            var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(key, serializerResponse, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }
    }
}
