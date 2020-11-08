using System.Collections.Generic;
using System.Threading.Tasks;

using RealEstateAgents.Application.DTOs.Property;

namespace RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers
{
    /// <summary>
    /// Helper for getting all the properties.
    /// </summary>
    public interface IPropertyDataHelper
    {
        Task<List<PropertyDto>> FetchAllProperties(string typeOfSearch, string searchQuery);
    }
}