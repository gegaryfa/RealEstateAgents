using System.Collections.Generic;
using System.Threading.Tasks;

using RealEstateAgents.Application.DTOs.Property;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;
using RealEstateAgents.Application.Interfaces.Services.Cache;

namespace RealEstateAgents.Infrastructure.Shared.Services.AgentService.Helpers
{
    public class PropertyDataHelperCacheDecorator : IPropertyDataHelper
    {
        private readonly IPropertyDataHelper _propertyDataHelper;
        private readonly ICacheService _cache;

        public PropertyDataHelperCacheDecorator(IPropertyDataHelper propertyDataHelper, ICacheService cache)
        {
            _propertyDataHelper = propertyDataHelper;
            _cache = cache;
        }

        public async Task<List<PropertyDto>> FetchAllProperties(string typeOfSearch, string searchQuery)
        {
            var cacheKey = $"{typeOfSearch}{searchQuery}";
            var cachedProperties = await _cache
                .Get(cacheKey, async () => await _propertyDataHelper.FetchAllProperties(typeOfSearch, searchQuery));

            return cachedProperties;
        }
    }
}