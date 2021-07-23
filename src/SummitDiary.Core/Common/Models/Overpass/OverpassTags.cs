using Newtonsoft.Json;

namespace SummitDiary.Core.Common.Models.Overpass
{
    public class OverpassTags
    {
        public string Ele { get; set; }
        public string Name { get; set; }
        public string Natural { get; set; }
        public string Wikidata { get; set; }
        public string Wikipedia { get; set; }
        public string Prominence { get; set; }
        [JsonProperty("summit:cross")]
        public string Cross { get; set; }
    }
}