using FloraFauna_GO_Dto.Internal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Mappers.Interfaces.Strategies;
using System.Text;
using System.Text.Json;

namespace FloraFauna_GO_Mappers.Strategies;

/// <summary>
/// Strategy for animal identification using the custom FloraFauna Animal API.
/// Acts as an Adapter, converting the custom API response to our normalized IdentificationApiResult format.
/// </summary>
public class AnimalApiStrategy : IIdentificationStrategy
{
    private readonly HttpClient _httpClient;
    private const string ENDPOINT = "http://codefirst.iut.uca.fr/containers/FloraFauna_GO-identification-api/FloraFaunaGo_API/identification/animal";

    public AnimalApiStrategy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IdentificationApiResult?> IdentifyAsync(byte[] imageBytes)
    {
        try
        {
            // Convert image to base64 as required by the custom Animal API
            var base64Image = Convert.ToBase64String(imageBytes);

            var requestBody = new
            {
                base64_image = base64Image
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // Clear headers for this specific API
            _httpClient.DefaultRequestHeaders.Clear();

            // Call custom Animal API
            var response = await _httpClient.PostAsync(ENDPOINT, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Animal API Error {response.StatusCode}: {errorContent}");
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Animal API Response: {jsonResponse}");

            // Deserialize custom Animal API response
            var animalResult = JsonSerializer.Deserialize<AnimalIdentificationResultDto>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (animalResult?.Predictions == null || !animalResult.Predictions.Any())
                return null;

            var bestPrediction = animalResult.Predictions.FirstOrDefault();
            if (bestPrediction?.Prediction == null)
                return null;

            // Extract species name from prediction string (format: "confidence;species_name")
            var speciesName = bestPrediction.Prediction.Split(';').LastOrDefault()?.Trim();
            
            if (string.IsNullOrEmpty(speciesName))
                return null;

            // Extract confidence score (first part before semicolon)
            var confidenceParts = bestPrediction.Prediction.Split(';');
            var score = 0.0;
            if (confidenceParts.Length > 1 && double.TryParse(confidenceParts[0], out var parsedScore))
            {
                score = parsedScore;
            }

            // Adapt to our normalized format
            return new IdentificationApiResult
            {
                SpeciesName = speciesName,
                ScientificName = null, // Custom API doesn't provide scientific names
                Score = score,
                PotentialReferenceImageUrl = null // Custom API doesn't provide reference images
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Animal API Strategy Error: {ex.Message}");
            return null;
        }
    }
}