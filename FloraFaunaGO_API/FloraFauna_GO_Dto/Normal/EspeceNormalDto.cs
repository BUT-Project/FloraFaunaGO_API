using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class EspeceNormalDto
{
    public Guid Id { get; set; }

    public string? Nom { get; set; }

    public string? Nom_Scientifique { get; set; }

    public string Description { get; set; }

    public Blob Image { get; set; }
}
