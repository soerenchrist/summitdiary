using System.Text.Json;
using System.Text.Json.Serialization;
using MonkeyCache.FileStore;
using SummitDiary.Core.Models.SummitAggregate;
using SummitDiary.Core.Models.SummitAggregate.Specs;
using Swashbuckle.AspNetCore.Annotations;

namespace SummitDiary.Web.ApiEndpoints.Summits;

public class GetImage : BaseAsyncEndpoint
    .WithRequest<int>
    .WithResponse<ImageResponseDto>
{
    private readonly IReadRepository<Summit> _summitRepository;

    public GetImage(IReadRepository<Summit> summitRepository)
    {
        _summitRepository = summitRepository;
    }
    
    [HttpGet("/api/summits/{summitId:int}/image")]
    [SwaggerOperation(
        Summary = "Get an image for the summit",
        Description = "Get an image using wikipedia data",
        OperationId = "Summit.GetImage",
        Tags = new[]{"SummitEndpoints"})]
    public override async Task<ActionResult<ImageResponseDto>> HandleAsync([FromRoute] int summitId, CancellationToken cancellationToken = new CancellationToken())
    {
        var cacheKey = $"summit-{summitId}";
        if(!Barrel.Current.IsExpired(cacheKey))
        {
            var url = Barrel.Current.Get<string>(cacheKey);
            return new ImageResponseDto
            {
                Url = url
            };
        }

        var summit = await _summitRepository.GetBySpecAsync(new GetSummitByIdSpec(summitId), cancellationToken);
        if (summit == null)
            return NotFound();
       
        var wikidata =summit.OsmData?.FirstOrDefault(x =>
                x.TagName == "wikidata");

        if (wikidata == null)
            return NotFound();
        
        var wikipediaFilename = await GetWikiFilename(wikidata.Value);
        if (wikipediaFilename == null)
            return NotFound();

        const int width = 500;
        var baseUrl = $"https://commons.wikimedia.org/w/thumb.php?width={width}&f={wikipediaFilename}";
            
        Barrel.Current.Add(cacheKey, baseUrl, TimeSpan.FromDays(30));
            
        return new ImageResponseDto
        {
            Url = baseUrl
        };
    }
    
    private async Task<string?> GetWikiFilename(string wikidataId)
    {
        var baseUrl = $"https://www.wikidata.org/w/api.php?action=wbgetclaims&property=P18&entity={wikidataId}&format=json";
        using var httpClient = new HttpClient();
        var data = await httpClient.GetStringAsync(baseUrl);

        var response = JsonSerializer.Deserialize<WikidataClaimsResponse>(data);
        if (response?.Claims?.Images.Count == 0)
            return null;

        return response?.Claims?.Images[0].Mainsnak?.DataValue?.Value;
    }
}


public class WikidataClaimsResponse
{
    [JsonPropertyName("claims")]
    public WikidataClaims? Claims { get; set; }
}

public class WikidataClaims
{
    [JsonPropertyName("P18")] public List<WikidataImage> Images { get; set; } = new();
}

public class WikidataImage
{
    [JsonPropertyName("mainsnak")]
    public WikidataSnak? Mainsnak { get; set; }
}

public class WikidataSnak
{
    [JsonPropertyName("datavalue")]
    public WikidataValue? DataValue { get; set; }
}

public class WikidataValue
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}