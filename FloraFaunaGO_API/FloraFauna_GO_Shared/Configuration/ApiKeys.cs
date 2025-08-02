namespace FloraFauna_GO_Shared.Configuration;

public class ApiKeys
{
    public const string SectionName = "ApiKeys";

    public string PlantNet { get; set; } = string.Empty;
    public string Kindwise { get; set; } = string.Empty;
    public string Gemini { get; set; } = string.Empty;
    public string GroqLlm { get; set; } = string.Empty;
}