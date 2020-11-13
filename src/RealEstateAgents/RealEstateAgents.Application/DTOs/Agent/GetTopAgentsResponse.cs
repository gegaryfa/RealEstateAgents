using System.Collections.Generic;

namespace RealEstateAgents.Application.DTOs.Agent
{
    public class GetTopAgentsResponse
    {
        public IEnumerable<AgentDto> Agents { get; set; }
    }
}