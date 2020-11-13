using AutoMapper;

using RealEstateAgents.Application.DTOs.Agent;
using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;

namespace RealEstateAgents.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AgentDto, GetTopAgentsViewModel>().ReverseMap();
            CreateMap<GetTopAgentsQuery, GetTopAgentsRequest>().ReverseMap();
        }
    }
}