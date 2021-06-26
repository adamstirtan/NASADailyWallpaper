using System;
using System.Text.Json.Serialization;

namespace NASADailyWallpaper.Client
{
    public record ApodResponse(
        [property: JsonPropertyName("copyright")] string Copyright,
        [property: JsonPropertyName("date")] DateTimeOffset Date,
        [property: JsonPropertyName("explanation")] string Explanation,
        [property: JsonPropertyName("hdurl")] string HDUrl,
        [property: JsonPropertyName("media_type")] string MediaType,
        [property: JsonPropertyName("service_version")] string ServiceVersion,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("url")] string Url);
}