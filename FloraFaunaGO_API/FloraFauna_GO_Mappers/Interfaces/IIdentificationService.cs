using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Mappers.Interfaces;

/// <summary>
/// Modern identification service that uses the Strategy pattern to delegate
/// species identification to appropriate external APIs.
/// This service orchestrates the entire identification workflow and handles
/// enrichment of new species data using Gemini API.
/// </summary>
public interface IIdentificationService
{
    /// <summary>
    /// Identifies a species from image bytes using the appropriate strategy.
    /// If the species is new, it will be enriched with additional data from Gemini API.
    /// </summary>
    /// <param name="imageBytes">The image data to analyze</param>
    /// <param name="type">The type of species to identify (Plant, Animal, etc.)</param>
    /// <returns>The identified species with enriched data, or null if identification fails</returns>
    Task<FullEspeceDto?> IdentifySpeciesAsync(byte[] imageBytes, EspeceType type);
}