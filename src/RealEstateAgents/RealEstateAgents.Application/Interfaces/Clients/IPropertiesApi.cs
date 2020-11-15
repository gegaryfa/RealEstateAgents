using System.Threading.Tasks;

using RealEstateAgents.Application.DTOs.Property;

using RestEase;

namespace RealEstateAgents.Application.Interfaces.Clients
{
    /// <summary>
    /// Interface for the the properties client.
    /// </summary>
    public interface IPropertiesApi
    {
        /// <summary>
        /// Get information about objects listed on funda.nl
        /// </summary>
        /// <param name="type">The type of search</param>
        /// <param name="zo">The search query</param>
        /// <param name="page">The page to retrieve.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns></returns>
        [Get("feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f")] //todo: get key from settings
        Task<Response<PropertiesApiResponse>> GetPropertiesForSaleAsync([Query] string type, [Query] string zo, [Query] int page, [Query] int pageSize);
    }
}