using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;

namespace RealEstateAgents.Application.DTOs.Agent
{
    public class GetTopAgentsRequest
    {
        public int NumberOfAgents { get; set; }
        public string Region { get; set; }
        public bool Garden { get; set; }
        public TypeOfSearch TypeOfSearch { get; set; }
    }
}