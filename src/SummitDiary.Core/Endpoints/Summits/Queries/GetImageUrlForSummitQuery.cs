using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Summits.Dto;

namespace SummitDiary.Core.Endpoints.Summits.Queries
{
    public class GetImageUrlForSummitQuery : IRequest<ImageResponseDto>
    {
        public int SummitId { get; }

        public GetImageUrlForSummitQuery(int summitId)
        {
            SummitId = summitId;
        }
    }

    public class GetImageUrlForSummitQueryHandler : IRequestHandler<GetImageUrlForSummitQuery, ImageResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetImageUrlForSummitQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<ImageResponseDto> Handle(GetImageUrlForSummitQuery request, CancellationToken cancellationToken)
        {
            var summit = await _context.Summits.FirstOrDefaultAsync(x => x.Id == request.SummitId, cancellationToken);
            if (summit == null)
                throw new NotFoundException(nameof(Summit), request.SummitId);

            var wikidata =
                await _context.OsmData.FirstOrDefaultAsync(x =>
                    x.TagName == "wikidata" && x.SummitId == request.SummitId, cancellationToken);

            if (wikidata == null)
                throw new NotFoundException(nameof(OsmData), request.SummitId);

            string wikipediaFilename = await GetWikiFilename(wikidata.Value);
            if (wikipediaFilename == null)
                throw new NotFoundException("Image", request.SummitId);

            const int width = 500;
            var baseUrl = $"https://commons.wikimedia.org/w/thumb.php?width={width}&f={wikipediaFilename}";

            return new ImageResponseDto
            {
                Url = baseUrl
            };
        }

        private async Task<string> GetWikiFilename(string wikidataId)
        {
            var baseUrl = $"https://www.wikidata.org/w/api.php?action=wbgetclaims&property=P18&entity={wikidataId}&format=json";
            using var httpClient = new HttpClient();
            var data = await httpClient.GetStringAsync(baseUrl);

            var response = JsonConvert.DeserializeObject<WikidataClaimsResponse>(data);
            if (response == null)
                return null;

            if (response.Claims?.Images.Count == 0)
                return null;

            return response.Claims?.Images[0].Mainsnak?.DataValue?.Value;
        }
    }

    public class WikidataClaimsResponse
    {
        [JsonProperty("claims")]
        public WikidataClaims Claims { get; set; }
    }

    public class WikidataClaims
    {
        [JsonProperty("P18")]
        public List<WikidataImage> Images { get; set; }
    }

    public class WikidataImage
    {
        [JsonProperty("mainsnak")]
        public WikidataSnak Mainsnak { get; set; }
    }

    public class WikidataSnak
    {
        [JsonProperty("datavalue")]
        public WikidataValue DataValue { get; set; }
    }

    public class WikidataValue
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}