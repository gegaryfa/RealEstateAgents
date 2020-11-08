using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;

namespace RealEstateAgents.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AgentsController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTopAgentsParameter filter)
        {
            return Ok(await Mediator.Send(new GetTopAgentsQuery
            {
                Region = filter.Region,
                Garden = filter.Garden,
                TypeOfSearch = filter.TypeOfSearch,
                NumberOfAgents = 10
            }));
        }
    }
}