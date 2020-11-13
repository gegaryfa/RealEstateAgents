using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RealEstateAgents.Application.Configurations;
using RealEstateAgents.Application.Enums;
using RealEstateAgents.Application.Interfaces.Services.Cache;

namespace RealEstateAgents.Infrastructure.Shared.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        private const CacheTechnology CacheTech = CacheTechnology.Memory;

        private readonly DistributedCacheEntryOptions _cacheOption;

        public CacheService(IOptions<CacheConfiguration> cacheConfig,
            Func<CacheTechnology, IDistributedCache> cacheServiceGetter)
        {
            _distributedCache = cacheServiceGetter(CacheTech);

            var cacheConfiguration = cacheConfig.Value;
            _cacheOption = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(cacheConfiguration.AbsoluteExpirationInHours))
                .SetSlidingExpiration(TimeSpan.FromMinutes(cacheConfiguration.SlidingExpirationInMinutes));
        }

        public async Task<T> Get<T>(string cacheKey, Func<Task<T>> factory)
        {
            var cachedValue = await _distributedCache.GetAsync(cacheKey);
            if (cachedValue == null)
            {
                var result = await factory();
                await this.Set(cacheKey, result);
                return result;
            }

            var serializedResult = Encoding.UTF8.GetString(cachedValue);
            var value = JsonConvert.DeserializeObject<T>(serializedResult);
            return value;
        }

        public async Task Set<T>(string cacheKey, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            var valueInBytes = Encoding.UTF8.GetBytes(serializedValue);
            await _distributedCache.SetAsync(cacheKey, valueInBytes, _cacheOption);
        }

        public async Task Remove(string cacheKey)
        {
            await _distributedCache.RemoveAsync(cacheKey);
        }
    }
}