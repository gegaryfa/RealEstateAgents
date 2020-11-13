using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using RealEstateAgents.Application.DTOs.Agent;
using RealEstateAgents.Application.Interfaces.Services.AgentService;

namespace RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents
{
    public class GetTopAgentsQuery : IRequest<IEnumerable<GetTopAgentsViewModel>>
    {
        public int NumberOfAgents { get; set; }
        public string Region { get; set; }
        public TypeOfSearch TypeOfSearch { get; set; }
        public bool Garden { get; set; }
    }

    public class GetTopAgentsQueryHandler : IRequestHandler<GetTopAgentsQuery, IEnumerable<GetTopAgentsViewModel>>
    {
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public GetTopAgentsQueryHandler(IMapper mapper, IAgentService agentService)
        {
            _mapper = mapper;
            this._agentService = agentService;
        }

        public async Task<IEnumerable<GetTopAgentsViewModel>> Handle(GetTopAgentsQuery query, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetTopAgentsRequest>(query);
            var topAgents = await _agentService.GetTopAgents(request);
            var topAgentsViewModels = _mapper.Map<IEnumerable<GetTopAgentsViewModel>>(topAgents.Agents);
            return topAgentsViewModels;
        }
    }
}