using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EnsureThat;

using RealEstateAgents.Application.DTOs.Agent;
using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;
using RealEstateAgents.Application.Interfaces.Services.AgentService;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;
using RealEstateAgents.Domain.Entities;

namespace RealEstateAgents.Infrastructure.Shared.Services.AgentService
{
    public class AgentService : IAgentService
    {
        private const string Buy = "koop";
        private const string Rent = "huur";
        private const string GardenQueryParam = "tuin";

        private readonly IPropertyDataHelper _propertyDataHelper;

        public AgentService(IPropertyDataHelper propertyDataHelper)
        {
            this._propertyDataHelper = propertyDataHelper;
        }

        public async Task<GetTopAgentsResponse> GetTopAgents(GetTopAgentsRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));
            // todo: refactor to a query builder
            var rentOrBuy = request.TypeOfSearch == TypeOfSearch.Buy ? Buy : Rent;

            var searchQuery = "/";
            searchQuery += request.Region + "/";

            searchQuery += request.Garden ? GardenQueryParam + "/" : "";
            // end refactor

            var allProperties = await this._propertyDataHelper.FetchAllProperties(rentOrBuy, searchQuery);

            var topAgents = GetTopAgentsDescending(request.NumberOfAgents, allProperties);

            return new GetTopAgentsResponse
            {
                Agents = topAgents
            };
        }

        private static IEnumerable<Agent> GetTopAgentsDescending(int numberOfAgents, IEnumerable<Property> allProperties)
        {
            var topList =
                allProperties
                    .GroupBy(o => o.Agent.Id)
                    .OrderByDescending(m => m.Count())
                    .Take(numberOfAgents)
                    .ToList();

            var topAgents = topList
                .Select(agent => agent.First())
                .Select(currentProperty => new Agent
                {
                    Id = currentProperty.Agent.Id,
                    Name = currentProperty.Agent.Name
                }).ToList();
            return topAgents;
        }
    }
}