using System.Text.Json.Serialization;

namespace FloraFauna_GO_Dto.Response;

public class InsectIdentificationResultDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("model_version")]
    public string ModelVersion { get; set; }

    [JsonPropertyName("custom_id")]
    public string CustomId { get; set; }

    public InputDto Input { get; set; }

    public InsectResultDto Result { get; set; }

    public string Status { get; set; }

    [JsonPropertyName("sla_compliant_client")]
    public bool SlaCompliantClient { get; set; }

    [JsonPropertyName("sla_compliant_system")]
    public bool SlaCompliantSystem { get; set; }

    public double Created { get; set; }

    public double Completed { get; set; }
}

public class InputDto
{
    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [JsonPropertyName("similar_images")]
    public bool SimilarImages { get; set; }

    public List<string> Images { get; set; }

    public DateTime Datetime { get; set; }
}

public class InsectResultDto
{
    public ClassificationDto Classification { get; set; }

    [JsonPropertyName("is_insect")]
    public IsInsectDto IsInsect { get; set; }
}

public class ClassificationDto
{
    public List<SuggestionDto> Suggestions { get; set; }
}

public class SuggestionDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public double Probability { get; set; }

    [JsonPropertyName("similar_images")]
    public List<SimilarImageDto> SimilarImages { get; set; }

    public SuggestionDetailsDto Details { get; set; }
}

public class SimilarImageDto
{
    public string Id { get; set; }

    public string Url { get; set; }

    public double Similarity { get; set; }

    [JsonPropertyName("url_small")]
    public string UrlSmall { get; set; }

    [JsonPropertyName("license_name")]
    public string LicenseName { get; set; }

    [JsonPropertyName("license_url")]
    public string LicenseUrl { get; set; }

    public string Citation { get; set; }
}

public class SuggestionDetailsDto
{
    [JsonPropertyName("common_names")]
    public List<string> CommonNames { get; set; }

    public string Url { get; set; }

    public DescriptionDto Description { get; set; }

    public ImageDto Image { get; set; }

    public string Language { get; set; }

    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; }
}

public class DescriptionDto
{
    public string Value { get; set; }

    public string Citation { get; set; }

    [JsonPropertyName("license_name")]
    public string LicenseName { get; set; }

    [JsonPropertyName("license_url")]
    public string LicenseUrl { get; set; }
}

public class ImageDto
{
    public string Value { get; set; }

    public string Citation { get; set; }

    [JsonPropertyName("license_name")]
    public string LicenseName { get; set; }

    [JsonPropertyName("license_url")]
    public string LicenseUrl { get; set; }
}

public class IsInsectDto
{
    public double Probability { get; set; }

    public double Threshold { get; set; }

    public bool Binary { get; set; }
}