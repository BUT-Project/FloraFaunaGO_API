using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class EspeceNormalDto
{
    public string? Id { get; set; }

    public string Nom { get; set; }

    public byte[]? Image { get; set; }

    public byte[]? Image3D { get; set; }

    public LocalisationNormalDto[]? Localisations { get; set; }
}
