using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents
{
    public class GetTopAgentsParameter
    {
        public string Region { get; set; }
        public bool Garden { get; set; }

        public TypeOfSearch TypeOfSearch { get; set; }

        public GetTopAgentsParameter()
        {
            this.Region = "amsterdam";
            this.Garden = false;
        }

        public GetTopAgentsParameter(string region, bool includingGarden)
        {
            this.Region = region;
            this.Garden = includingGarden;
        }
    }

    public enum TypeOfSearch
    {
        [JsonConverter(typeof(StringEnumConverter))]
        Buy,

        [JsonConverter(typeof(StringEnumConverter))]
        Rent
    }
}