using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace NASADailyWallpaper.Client
{
    public sealed class NasaApodClient : INasaApodClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "ARtBNeywWICmBZgcQAjMHdxmcwApSnLTeeOGOC49";

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            AllowTrailingCommas = false,
            PropertyNameCaseInsensitive = true
        };

        public NasaApodClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.nasa.gov/planetary/apod")
            };

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ApodResponse> GetLatestImage()
        {
            try
            {
                HttpRequestMessage request = new(HttpMethod.Get, $"?date={DateTime.Now:yyyy-MM-dd}&api_key={_apiKey}");

                using HttpResponseMessage response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                return await JsonSerializer.DeserializeAsync<ApodResponse>(
                    await response.Content.ReadAsStreamAsync(), JsonSerializerOptions);
            }
            catch
            {
                return null;
            }
        }
    }
}