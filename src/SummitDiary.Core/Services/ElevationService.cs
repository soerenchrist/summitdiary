using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using SummitDiary.Core.Common.Interfaces;

namespace SummitDiary.Core.Services
{
    public class ElevationService : IElevationService
    {
        private readonly HttpClient _httpClient;

        public ElevationService()
        {
            _httpClient = new HttpClient();
        }
        
        public async Task<double?> GetElevation(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            var cacheKey =
                $"elevation-{Math.Round(latitude, 4).ToString(CultureInfo.InvariantCulture)},{Math.Round(longitude, 4).ToString(CultureInfo.InvariantCulture)}";
            string json;
            if (!Barrel.Current.IsExpired(cacheKey))
            {
                json = Barrel.Current.Get<string>(cacheKey);
            }
            else
            {
                json = await FetchElevationFromApi(latitude, longitude, cancellationToken);
            }

            if (json == null)
                return null;

            var response = JsonSerializer.Deserialize<ElevationResponse>(json);
            if (response == null)
                return null;
            
            Barrel.Current.Add(cacheKey, json, TimeSpan.MaxValue);

            if (response.Results is {Count: > 0})
            {
                return response.Results[0].Elevation;
            }

            return null;
        }

        private async Task<string> FetchElevationFromApi(double latitude, double longitude,
            CancellationToken cancellationToken)
        {
            var latString = latitude.ToString(CultureInfo.InvariantCulture);
            var lonString = longitude.ToString(CultureInfo.InvariantCulture);
            var baseUrl = $"https://api.open-elevation.com/api/v1/lookup?locations={latString},{lonString}";
            var response = await _httpClient.GetAsync(baseUrl, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }

    public class ElevationResponse
    {
        [JsonPropertyName("results")]
        public List<ElevationResult> Results { get; set; }
    }

    public class ElevationResult
    {
        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }
    }
}