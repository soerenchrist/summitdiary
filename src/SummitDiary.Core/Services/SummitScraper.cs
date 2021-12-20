using System.Globalization;
using Newtonsoft.Json;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Common.Models.Overpass;

namespace SummitDiary.Core.Services
{
    public class SummitScraper
    {
        public async Task<List<Summit>> FetchSummits()
        {
            const string basePath = "Resources";
            var files = new Dictionary<int, string>()
            {
                {1, "germany.json"},
                {2, "austria.json"},
                {3, "switzerland.json"},
                {4, "france.json"},
                {5, "italy.json"},
            };

            var summits = new List<Summit>();
            foreach (var file in files)
            {
                Console.WriteLine($"Reading summits from {file.Value}");
                var path = Path.Combine(basePath, file.Value);
                
                if (!File.Exists(path))
                    continue;

                var content = await File.ReadAllTextAsync(path);
                var response = JsonConvert.DeserializeObject<OverpassResult>(content);
                var count = response.Elements?.Count();
                Console.WriteLine($"Found {count} mountains");

                var counter = 0;
                foreach (var element in response.Elements ?? new List<OverpassElement>())
                {
                    counter++;
                    if (string.IsNullOrWhiteSpace(element.Tags?.Ele)) continue;
                    if (string.IsNullOrWhiteSpace(element.Tags.Name)) continue;

                    if (!double.TryParse(element.Tags.Ele, NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var height))
                        continue;
                    
                    if (height < 2500)
                        continue;
                    
                    Console.WriteLine($"Processing {element.Tags.Name}: {counter}/{count}");

                    // Possibly existing summit in a different country
                    var existing = summits.FirstOrDefault(x => x.Name == element.Tags.Name
                                                && Math.Abs(x.Latitude - element.Lat) < 0.00001 &&
                                                Math.Abs(x.Longitude - element.Lon) < 0.00001);
                    if (existing != null)
                        continue;

                    var summit = new Summit
                    {
                        Name = element.Tags.Name,
                        Height = (int) height,
                        Latitude = element.Lat,
                        Longitude = element.Lon,
                        OpenStreetmapId = element.Id,
                        CountryId = file.Key,
                        RegionId = 1,
                        OsmData = GetOsmData(element),
                    };
                    summits.Add(summit);
                }
            }

            return summits;
        }

        private IEnumerable<OsmData> GetOsmData(OverpassElement element)
        {
            var tags = new List<OsmData>();
            if (!string.IsNullOrWhiteSpace(element.Tags?.Ele))
                tags.Add(new OsmData{TagName = "ele", Value = element.Tags.Ele, OpenStreetMapId = element.Id});
            if (!string.IsNullOrWhiteSpace(element.Tags?.Prominence))
                tags.Add(new OsmData{TagName = "prominence", Value = element.Tags.Prominence, OpenStreetMapId = element.Id});
            if (!string.IsNullOrWhiteSpace(element.Tags?.Wikidata))
                tags.Add(new OsmData{TagName = "wikidata", Value = element.Tags.Wikidata, OpenStreetMapId = element.Id});
            if (!string.IsNullOrWhiteSpace(element.Tags?.Wikipedia))
                tags.Add(new OsmData{TagName = "wikipedia", Value = element.Tags.Wikipedia, OpenStreetMapId = element.Id});
            if (!string.IsNullOrWhiteSpace(element.Tags?.Cross))
                tags.Add(new OsmData{TagName = "summit:cross", Value = element.Tags.Cross, OpenStreetMapId = element.Id});

            return tags;
        }
    }
}