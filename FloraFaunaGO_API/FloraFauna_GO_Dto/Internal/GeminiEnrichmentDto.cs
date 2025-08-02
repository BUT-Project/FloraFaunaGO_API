namespace FloraFauna_GO_Dto.Internal;

/// <summary>
/// DTO for structured data enrichment from Gemini API using JSON Mode.
/// This corresponds to the response schema we define for Gemini to ensure reliable, parseable responses.
/// </summary>
public class GeminiEnrichmentDto
{
    public string nom { get; set; } = string.Empty;
    public string nom_scientifique { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string famille { get; set; } = string.Empty;
    public string zone { get; set; } = string.Empty;
    public string climat { get; set; } = string.Empty;
    public string @class { get; set; } = string.Empty;
    public string kingdom { get; set; } = string.Empty;
    public string regime { get; set; } = string.Empty;
}