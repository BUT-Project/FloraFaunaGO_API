using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class CaptureNormalDto
{
    public string? Id { get; set; }

    public string IdEspece { get; set; }
    public byte[]? photo { get; set; }

    public LocalisationNormalDto? LocalisationNormalDto { get; set; }

    public bool? Shiny { get; set; }
}
