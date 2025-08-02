using FloraFauna_GO_Dto.Internal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Mappers.Interfaces.Strategies;
using FloraFauna_GO_Shared.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FloraFauna_GO_Mappers.Strategies;

/// <summary>
/// Strategy for plant identification using PlantNet API.
/// Acts as an Adapter, converting PlantNet-specific responses to our normalized IdentificationApiResult format.
/// </summary>
public class PlantNetStrategy : IIdentificationStrategy
{
    private readonly HttpClient _httpClient;
    private readonly ApiKeys _apiKeys;
    private readonly string _endpoint;

    public PlantNetStrategy(HttpClient httpClient, IOptions<ApiKeys> apiKeysOptions)
    {
        _httpClient = httpClient;
        _apiKeys = apiKeysOptions.Value;
        _endpoint = $"https://my-api.plantnet.org/v2/identify/all?lang=fr&api-key={_apiKeys.PlantNet}";
    }

    public async Task<IdentificationApiResult?> IdentifyAsync(byte[] imageBytes)
    {
        try
        {
            // Prepare multipart form data as required by PlantNet API
            using var form = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            form.Add(imageContent, "images", "image.jpg");

            // Call PlantNet API
            var response = await _httpClient.PostAsync(_endpoint, form);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"PlantNet API Error: {response.StatusCode}");
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"PlantNet API Response: {jsonResponse}");

            // Deserialize PlantNet-specific response
            var plantResult = JsonSerializer.Deserialize<PlantIdentificationResultDto>(
                jsonResponse, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (plantResult?.Results == null || !plantResult.Results.Any())
                return null;

            // Get the highest scoring result
            var bestResult = plantResult.Results.OrderByDescending(r => r.Score).FirstOrDefault();
            if (bestResult?.Species?.CommonNames == null || !bestResult.Species.CommonNames.Any())
                return null;

            // Adapt to our normalized format
            return new IdentificationApiResult
            {
                SpeciesName = bestResult.Species.CommonNames.FirstOrDefault() ?? string.Empty,
                ScientificName = bestResult.Species.ScientificNameWithoutAuthor,
                Score = bestResult.Score,
                PotentialReferenceImageUrl = null // PlantNet doesn't provide reference images in standard response
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PlantNet Strategy Error: {ex.Message}");
            return null;
        }
    }
}