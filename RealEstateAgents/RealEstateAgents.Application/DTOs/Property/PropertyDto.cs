using Newtonsoft.Json;

namespace RealEstateAgents.Application.DTOs.Property
{
    public class PropertyDto
    {
        [JsonProperty("MakelaarId")]
        public int AgentId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string AgentName { get; set; }
    }
}