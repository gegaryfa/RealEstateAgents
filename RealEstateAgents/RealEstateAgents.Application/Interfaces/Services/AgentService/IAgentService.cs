using System.Threading.Tasks;

using RealEstateAgents.Application.DTOs.Agent;

namespace RealEstateAgents.Application.Interfaces.Services.AgentService
{
    public interface IAgentService
    {
        Task<GetTopAgentsResponse> GetTopAgents(GetTopAgentsRequest request);
    }
}