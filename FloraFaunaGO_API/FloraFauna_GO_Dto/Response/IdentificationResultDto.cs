using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Response;

public class IdentificationResultDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
    public GenusDto Genus { get; set; }
    public FamilyDto Family { get; set; }
    public List<string> CommonNames { get; set; }
    public double Score { get; set; }
}

public class GenusDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
}

public class FamilyDto
{
    public string ScientificNameWithoutAuthor { get; set; }
    public string ScientificNameAuthorship { get; set; }
}
