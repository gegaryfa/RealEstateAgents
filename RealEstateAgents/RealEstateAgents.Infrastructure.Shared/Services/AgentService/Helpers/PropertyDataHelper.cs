using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

        private readonly IPropertiesApi _propertiesApi;
        private readonly ILogger<PropertyDataHelper> _logger;
        private const int PageSize = 25;

        public PropertyDataHelper(IPropertiesApi propertiesApi, ILogger<PropertyDataHelper> logger)
        {
            _propertiesApi = propertiesApi;
            _logger = logger;
        }

        public async Task<List<PropertyDto>> FetchAllProperties(string typeOfSearch, string searchQuery)
        {
            var totalObjects = new List<PropertyDto>();

            // Get the first page in order to find the total number of pages
            var firstApiResponseContent = await GetApiResponseContentForPageAsync(1, PageSize, typeOfSearch, searchQuery);

            // Add the objects of the current page to the total amount of objects
            totalObjects.AddRange(firstApiResponseContent.Properties);

            var totalPages = Enumerable.Range(2, firstApiResponseContent.Paging.NumberOfPages).ToList();

            // Setup the rest of the tasks
            var getApiResponseTaskQuery =
                from page in totalPages select GetApiResponseContentForPageAsync(page, PageSize, typeOfSearch, searchQuery);

            // Use ToList to execute the query and start the tasks.
            var apiResponseTasks = getApiResponseTaskQuery.ToList();

            // Process the tasks one at a time until none remain.
            while (apiResponseTasks.Count > 0)
            {
                // Identify the first task that completes.
                var finishedTask = await Task.WhenAny(apiResponseTasks);

                // Remove the selected task from the list so that you don't process it more than once.
                apiResponseTasks.Remove(finishedTask);

                // Await the completed task.
                var apiResponseContent = await finishedTask;

                // Add the object of the page in the total objects
                totalObjects.AddRange(apiResponseContent.Properties);
            }

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