using FloraFauna_GO_Dto.Internal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Mappers.Interfaces.Strategies;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FloraFauna_GO_Mappers.Strategies;

/// <summary>
/// Strategy for insect identification using Kindwise API.
/// Acts as an Adapter, converting Kindwise-specific responses to our normalized IdentificationApiResult format.
/// </summary>
public class KindwiseStrategy : IIdentificationStrategy
{
    private readonly HttpClient _httpClient;
    private readonly ApiKeys _apiKeys;
    private const string ENDPOINT = "https://insect.kindwise.com/api/v1/identification?details=common_names,url,description,image";

    public KindwiseStrategy(HttpClient httpClient, IOptions<ApiKeys> apiKeysOptions)
    {
        _httpClient = httpClient;
        _apiKeys = apiKeysOptions.Value;
    }

    public async Task<IdentificationApiResult?> IdentifyAsync(byte[] imageBytes)
    {
        try
        {
            // Convert image to base64 as required by Kindwise API
            var base64Image = Convert.ToBase64String(imageBytes);

            var requestBody = new
            {
                images = new[] { base64Image }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // Set API key header
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Api-Key", _apiKeys.Kindwise);

            // Call Kindwise API
            var response = await _httpClient.PostAsync(ENDPOINT, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kindwise API Error {response.StatusCode}: {errorContent}");
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Kindwise API Response: {jsonResponse}");

            // Deserialize Kindwise-specific response
            var insectResult = JsonSerializer.Deserialize<InsectIdentificationResultDto>(
                jsonResponse, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var bestSuggestion = insectResult?.Result?.Classification?.Suggestions
                ?.OrderByDescending(s => s.Probability)
                ?.FirstOrDefault();

            if (bestSuggestion == null)
                return null;

            // Adapt to our normalized format
            return new IdentificationApiResult
            {
                SpeciesName = bestSuggestion.Name ?? string.Empty,
                ScientificName = bestSuggestion.Name, // Kindwise doesn't provide separate scientific name
                Score = bestSuggestion.Probability,
                PotentialReferenceImageUrl = bestSuggestion.Details?.Image?.Value
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kindwise Strategy Error: {ex.Message}");
            return null;
        }
    }
}