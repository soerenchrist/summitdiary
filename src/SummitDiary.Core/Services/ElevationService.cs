using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        
        public async Task<double?> GetElevation(double latitude, double longitude)
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
                json = await FetchElevationFromApi(latitude, longitude);
            }

            if (json == null)
                return null;

            var response = JsonSerializer.Deserialize<ElevationResponse>(json);
            if (response == null)
                return null;
            
            Barrel.Current.Add(cacheKey, json, TimeSpan.MaxValue);

            if (response.Status == "OK" && response.Results.Count > 0)
            {
                return response.Results[0].Elevation;
            }

            return null;
        }

        private Task<string> FetchElevationFromApi(double latitude, double longitude)
        {
            var latString = latitude.ToString(CultureInfo.InvariantCulture);
            var lonString = longitude.ToString(CultureInfo.InvariantCulture);
            var baseUrl = $"https://api.opentopodata.org/v1/srtm90m?locations={latString},{lonString}&interpolation=cubic";
            return _httpClient.GetStringAsync(baseUrl);
        }
    }

    public class ElevationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("results")]
        public List<ElevationResult> Results { get; set; }
    }

    public class ElevationResult
    {
        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }
    }
}