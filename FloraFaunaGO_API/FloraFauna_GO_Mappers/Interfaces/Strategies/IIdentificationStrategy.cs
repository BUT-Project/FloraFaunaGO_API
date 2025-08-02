using FloraFauna_GO_Dto.Internal;

namespace FloraFauna_GO_Mappers.Interfaces.Strategies;

/// <summary>
/// Strategy interface for different species identification providers.
/// Each strategy encapsulates the logic for communicating with a specific
/// identification API (PlantNet, Kindwise, etc.) and adapts their responses
/// to our common IdentificationApiResult format.
/// </summary>
public interface IIdentificationStrategy
{
    /// <summary>
    /// Identifies a species from image bytes using the specific provider's API.
    /// </summary>
    /// <param name="imageBytes">The image data to analyze</param>
    /// <returns>Normalized identification result or null if identification fails</returns>
    Task<IdentificationApiResult?> IdentifyAsync(byte[] imageBytes);
}