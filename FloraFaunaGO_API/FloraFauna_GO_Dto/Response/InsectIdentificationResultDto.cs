using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Response;

public class InsectIdentificationResultDto
{
    public string AccessToken { get; set; }

    public string ModelVersion { get; set; }

    public string CustomId { get; set; }

    public InputDto Input { get; set; }

    public InsectResultDto Result { get; set; }

    public string Status { get; set; }

    public bool SlaCompliantClient { get; set; }

    public bool SlaCompliantSystem { get; set; }

    public double Created { get; set; }

    public double Completed { get; set; }
}

public class InputDto
{
    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool SimilarImages { get; set; }

    public List<string> Images { get; set; }

    public DateTime Datetime { get; set; }
}

public class InsectResultDto
{
    public ClassificationDto Classification { get; set; }

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

    public List<SimilarImageDto> SimilarImages { get; set; }

    public SuggestionDetailsDto Details { get; set; }
}

public class SimilarImageDto
{
    public string Id { get; set; }

    public string Url { get; set; }

    public double Similarity { get; set; }

    public string UrlSmall { get; set; }

    public string LicenseName { get; set; }

    public string LicenseUrl { get; set; }

    public string Citation { get; set; }
}

public class SuggestionDetailsDto
{
    public List<string> CommonNames { get; set; }

    public string Url { get; set; }

    public DescriptionDto Description { get; set; }

    public ImageDto Image { get; set; }

    public string Language { get; set; }

    public string EntityId { get; set; }
}

public class DescriptionDto
{
    public string Value { get; set; }

    public string Citation { get; set; }

    public string LicenseName { get; set; }

    public string LicenseUrl { get; set; }
}

public class ImageDto
{
    public string Value { get; set; }

    public string Citation { get; set; }

    public string LicenseName { get; set; }

    public string LicenseUrl { get; set; }
}

public class IsInsectDto
{
    public double Probability { get; set; }

    public double Threshold { get; set; }

    public bool Binary { get; set; }
}

