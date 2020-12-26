using System.Collections.Generic;
using System.Threading.Tasks;

using RealEstateAgents.Domain.Entities;

namespace RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers
{
    /// <summary>
    /// Helper for getting all the properties.
    /// </summary>
    public interface IPropertyDataHelper
    {
        Task<List<Property>> FetchAllProperties(string typeOfSearch, string searchQuery);
    }
}