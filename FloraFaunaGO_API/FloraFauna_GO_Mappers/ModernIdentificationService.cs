using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Internal;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Mappers.Factories;
using FloraFauna_GO_Mappers.Interfaces;
using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;

namespace FloraFauna_GO_Mappers;

/// <summary>
/// Modern implementation of the IdentificationService using Strategy pattern.
/// This service orchestrates species identification by delegating to appropriate strategies
/// and handles enrichment of new species data using Gemini API with JSON Mode.
/// </summary>
public class ModernIdentificationService : IIdentificationService
{
    private readonly IdentificationStrategyFactory _strategyFactory;
    private readonly IEspeceRepository<FullEspeceDto, FullEspeceDto> _especeRepository;
    private readonly HttpClient _httpClient;
    private readonly ApiKeys _apiKeys;

    // Gemini API endpoint for structured JSON responses
    private const string GEMINI_ENDPOINT = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";

    public ModernIdentificationService(
        IdentificationStrategyFactory strategyFactory,
        FloraFaunaService floraFaunaService,
        HttpClient httpClient,
        IOptions<ApiKeys> apiKeysOptions)
    {
        _strategyFactory = strategyFactory;
        _especeRepository = floraFaunaService.EspeceRepository;
        _httpClient = httpClient;
        _apiKeys = apiKeysOptions.Value;
    }

    public async Task<FullEspeceDto?> IdentifySpeciesAsync(byte[] imageBytes, EspeceType type)
    {
        try
        {
            // Step 1: Get the appropriate strategy and perform identification
            var strategy = _strategyFactory.CreateStrategy(type);
            var apiResult = await strategy.IdentifyAsync(imageBytes);

            if (apiResult == null || string.IsNullOrEmpty(apiResult.SpeciesName))
                return null;

            Console.WriteLine($"Species identified: {apiResult.SpeciesName} (Score: {apiResult.Score})");

            // Step 2: Check if species already exists in database
            var existingSpecies = await _especeRepository.GetEspeceByName(apiResult.SpeciesName);
            if (existingSpecies?.Items != null && existingSpecies.Items.Any())
            {
                Console.WriteLine($"Species '{apiResult.SpeciesName}' found in database");
                return existingSpecies.Items.First();
            }

            // Step 3: Species is new, enrich with Gemini data using JSON Mode
            Console.WriteLine($"New species '{apiResult.SpeciesName}' - enriching with Gemini...");
            var enrichmentData = await RetrieveFloraFaunaDatasFromGemini(apiResult.SpeciesName);

            if (enrichmentData == null)
            {
                Console.WriteLine("Failed to enrich species data with Gemini");
                return null;
            }

            // Step 4: Create new FullEspeceDto with enriched data
            var newSpecies = new FullEspeceDto
            {
                Id = null, // Will be set by database on insert
                Nom = apiResult.SpeciesName,
                Nom_Scientifique = apiResult.ScientificName ?? enrichmentData.nom_scientifique,
                Description = enrichmentData.description,
                ImageUrl = apiResult.PotentialReferenceImageUrl, // From identification API
                Image3DUrl = null, // Not provided by identification APIs
                Famille = enrichmentData.famille,
                Zone = enrichmentData.zone,
                Climat = enrichmentData.climat,
                Class = enrichmentData.@class,
                Kingdom = enrichmentData.kingdom,
                Regime = enrichmentData.regime,
                localisations = null // Will be handled separately if needed
            };

            Console.WriteLine($"Species enriched successfully: {newSpecies.Nom}");
            return newSpecies;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in species identification: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Enriches species data using Gemini API with structured JSON Mode for reliable responses.
    /// This implementation uses the response_schema feature to ensure consistent, parseable JSON output.
    /// </summary>
    private async Task<GeminiEnrichmentDto?> RetrieveFloraFaunaDatasFromGemini(string speciesName)
    {
        try
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"Provide detailed information about the species '{speciesName}'. Include taxonomic classification, habitat, diet, and description." }
                        }
                    }
                },
                generationConfig = new
                {
                    response_mime_type = "application/json",
                    response_schema = new
                    {
                        type = "object",
                        properties = new
                        {
                            nom = new { type = "string", description = "Common name of the species" },
                            nom_scientifique = new { type = "string", description = "Scientific binomial name" },
                            description = new { type = "string", description = "Brief description of the species" },
                            famille = new { type = "string", description = "Taxonomic family" },
                            zone = new { type = "string", description = "Geographic zone or habitat" },
                            climat = new { type = "string", description = "Climate preferences" },
                            @class = new { type = "string", description = "Taxonomic class" },
                            kingdom = new { type = "string", description = "Taxonomic kingdom (Animalia, Plantae, etc.)" },
                            regime = new { type = "string", description = "Diet or feeding habits" }
                        },
                        required = new[] { "nom", "nom_scientifique", "famille", "class", "kingdom" }
                    }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"{GEMINI_ENDPOINT}?key={_apiKeys.Gemini}";
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Gemini API Error {response.StatusCode}: {errorContent}");
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Gemini API Response: {jsonResponse}");

            // Parse Gemini response structure
            using var document = JsonDocument.Parse(jsonResponse);
            var candidatesArray = document.RootElement.GetProperty("candidates");
            if (candidatesArray.GetArrayLength() == 0)
                return null;

            var content = candidatesArray[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
                return null;

            // Since we're using JSON Mode, the response should be clean JSON
            var enrichmentData = JsonSerializer.Deserialize<GeminiEnrichmentDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return enrichmentData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enriching species data with Gemini: {ex.Message}");
            return null;
        }
    }
}