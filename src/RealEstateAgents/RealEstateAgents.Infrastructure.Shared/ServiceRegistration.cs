using System;
using System.Linq;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RealEstateAgents.Application.Configurations;
using RealEstateAgents.Application.Enums;
using RealEstateAgents.Application.Interfaces.Clients;
using RealEstateAgents.Application.Interfaces.Services.AgentService;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;
using RealEstateAgents.Application.Interfaces.Services.Cache;
using RealEstateAgents.Infrastructure.Shared.Services.AgentService;
using RealEstateAgents.Infrastructure.Shared.Services.AgentService.Helpers;
using RealEstateAgents.Infrastructure.Shared.Services.Cache;

using RestEase;

namespace RealEstateAgents.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // start Caching
            services.Configure<CacheConfiguration>(config.GetSection("CacheConfiguration"));

            services.AddDistributedMemoryCache();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = config.GetConnectionString("Redis");
            });

            services.AddTransient<ICacheService, CacheService>();

            // This is required if you want to support multiple type of caching technologies on the same project.
            // In cases where only one caching mechanism is required, then you can remove this piece of code and
            // register only the required service for the desired type of caching(redis,inMemory,Sql...).
            services.AddTransient<Func<CacheTechnology, IDistributedCache>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTechnology.Memory:
                        return serviceProvider.GetServices<IDistributedCache>().First(c => c is MemoryDistributedCache);

                    case CacheTechnology.Redis:
                        return serviceProvider.GetServices<IDistributedCache>().First(c => c is RedisCache);

                    default:
                        return serviceProvider.GetServices<IDistributedCache>().First(c => c is MemoryDistributedCache);
                }
            });
            // End caching

            services.AddTransient<IPropertyDataHelper, PropertyDataHelper>();
            services.Decorate<IPropertyDataHelper, PropertyDataHelperCacheDecorator>();

            services.AddTransient<IAgentService, AgentService>();

            services.AddSingleton(
                serviceProvider =>
                {
                    var basePath = config["PropertiesApi:basePath"];
                    var client = RestClient.For<IPropertiesApi>(basePath);
                    return client;
                });
        }
    }
}