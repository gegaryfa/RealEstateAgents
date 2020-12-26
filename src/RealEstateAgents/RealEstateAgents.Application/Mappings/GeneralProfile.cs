using AutoMapper;

using RealEstateAgents.Application.DTOs.Agent;
using RealEstateAgents.Application.DTOs.Property;
using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;
using RealEstateAgents.Domain.Entities;

namespace RealEstateAgents.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Agent, GetTopAgentsViewModel>().ReverseMap();

            ConfigurePropertyDtoMapping();

            CreateMap<GetTopAgentsQuery, GetTopAgentsRequest>().ReverseMap();
        }

        private void ConfigurePropertyDtoMapping()
        {
            CreateMap<PropertyDto, Property>()
                .ForMember(dest => dest.Agent,
                    opt => opt.MapFrom(src => src));

            // this mapping is required to map the <PropertyDto> to the property <Agent> of the class <Property>
            CreateMap<PropertyDto, Agent>()
                .ForMember(d => d.Id,
                    opt => opt.MapFrom(src => src.AgentId))
                .ForMember(d => d.Name,
                    opt => opt.MapFrom(src => src.AgentName));
        }
    }
}