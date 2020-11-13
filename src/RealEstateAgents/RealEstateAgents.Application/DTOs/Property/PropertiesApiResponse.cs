using System.Collections.Generic;

using Newtonsoft.Json;

namespace RealEstateAgents.Application.DTOs.Property
{
    public class PropertiesApiResponse
    {
        [JsonProperty("Objects")]
        public List<PropertyDto> Properties { get; set; }

        [JsonProperty("Paging")]
        public Paging Paging { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public int TotalNumberOfProperties { get; set; }
    }

    public class Paging
    {
        [JsonProperty("AantalPaginas")]
        public int NumberOfPages { get; set; }

        [JsonProperty("HuidigePagina")]
        public int CurrentPage { get; set; }
    }
}