using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Polly;

using RealEstateAgents.Application.DTOs.Property;
using RealEstateAgents.Application.Interfaces.Clients;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;

using RestEase;

namespace RealEstateAgents.Infrastructure.Shared.Services.AgentService.Helpers
{
    public class PropertyDataHelper : IPropertyDataHelper
    {
        private const int ApiRequestMaxRetries = 5;

        private const string PageSizeAppSettingKey = "PropertiesApi:pageSize";

        private readonly IPropertiesApi _propertiesApi;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PropertyDataHelper> _logger;

        public PropertyDataHelper(IPropertiesApi propertiesApi, IConfiguration configuration, ILogger<PropertyDataHelper> logger)
        {
            _propertiesApi = propertiesApi;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<PropertyDto>> FetchAllProperties(string typeOfSearch, string searchQuery)
        {
            var totalObjects = new List<PropertyDto>();

            int.TryParse(_configuration[PageSizeAppSettingKey], out var pageSize);

            var currentPage = 1;
            var totalPages = 0;

            do
            {
                var apiResponseContent = await this.GetApiResponseContentForPageAsync(currentPage, pageSize, typeOfSearch, searchQuery);

                // Add the objects of the current page to the total amount of objects
                totalObjects.AddRange(apiResponseContent.Properties);

                totalPages = apiResponseContent.Paging.NumberOfPages;

                currentPage++;
            } while (currentPage <= totalPages);

            return totalObjects;
        }

        private async Task<PropertiesApiResponse> GetApiResponseContentForPageAsync(int currentPage, int pageSize, string typeOfSearch, string searchQuery)
        {
            try
            {
                // Retry up to 5 times with exponential backoff. This can also be configured in a custom httpClient that
                // will be used to construct the Restease Api client.
                using var apiResponse = await Policy
                    .Handle<ApiException>(p => p.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    .WaitAndRetryAsync(ApiRequestMaxRetries, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                        {
                            // log it
                            _logger.LogWarning($"Request failed with {exception.Message}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                        })
                    .ExecuteAsync(() => _propertiesApi.GetPropertiesForSaleAsync(typeOfSearch, searchQuery, currentPage, pageSize));

                if (!apiResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    var errorResult = JsonConvert.DeserializeObject<ApiException>(apiResponse.StringContent);
                    throw errorResult;
                }

                var result = apiResponse.GetContent();
                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError("Request failed due to " + ex.ReasonPhrase, ex);
                throw;
            }
        }
    }
}