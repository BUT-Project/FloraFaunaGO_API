namespace FloraFauna_GO_Dto.Internal;

/// <summary>
/// Normalized result from any identification API.
/// This DTO serves as the common interface between all identification strategies and the rest of the application.
/// Each strategy acts as an Adapter, converting its specific API response into this standardized format.
/// </summary>
public class IdentificationApiResult
{
    /// <summary>
    /// The common name of the identified species
    /// </summary>
    public string SpeciesName { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL to a reference image of the species, if provided by the API
    /// </summary>
    public string? PotentialReferenceImageUrl { get; set; }

    /// <summary>
    /// Confidence score of the identification (0.0 to 1.0)
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    /// Scientific name if available from the API
    /// </summary>
    public string? ScientificName { get; set; }
}