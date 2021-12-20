using Newtonsoft.Json;

namespace SummitDiary.Core.Common.Models.Overpass
{
    public class OverpassTags
    {
        public string Ele { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Natural { get; set; } = string.Empty;
        public string? Wikidata { get; set; }
        public string? Wikipedia { get; set; }
        public string? Prominence { get; set; }
        [JsonProperty("summit:cross")]
        public string? Cross { get; set; }
    }
}