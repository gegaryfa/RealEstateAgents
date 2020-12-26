using System.Collections.Generic;

namespace RealEstateAgents.Application.DTOs.Agent
{
    public class GetTopAgentsResponse
    {
        public IEnumerable<Domain.Entities.Agent> Agents { get; set; }
    }
}