using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Response;

public class IdentificationResultDto
{
    public QueryDto Query { get; set; }
    public List<PredictedOrganDto> PredictedOrgans { get; set; }
    public string Language { get; set; }
    public string PreferedReferential { get; set; }
    public string BestMatch { get; set; }
    public List<ResultDto> Results { get; set; }
    public string Version { get; set; }
    public int RemainingIdentificationRequests { get; set; }
}

public class QueryDto
{
    public string Project { get; set; }
    public List<string> Images { get; set; }
    public List<string> Organs { get; set; }
    public bool IncludeRelatedImages { get; set; }
    public bool NoReject { get; set; }
    public object Type { get; set; }
}

public class PredictedOrganDto
{
    public string Image { get; set; }
    public string Filename { get; set; }
    public string Organ { get; set; }
    public double Score { get; set; }
}

public class ResultDto
{
    public double Score { get; set; }
    public SpeciesDto Species { get; set; }
    public IdentifierDto Gbif { get; set; }
    public IdentifierDto Powo { get; set; }
    public IucnDto Iucn { get; set; } // Nullable, car toutes les entrées ne l'ont pas
}

public class SpeciesDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
    public GenusDto Genus { get; set; }
    public FamilyDto Family { get; set; }
    public List<string> CommonNames { get; set; }
    public string ScientificName { get; set; }
}

public class GenusDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
    public string ScientificName { get; set; }
}

public class FamilyDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
    public string ScientificName { get; set; }
}

public class IdentifierDto
{
    public string Id { get; set; }
}

public class IucnDto
{
    public string Id { get; set; }
    public string Category { get; set; }
}
