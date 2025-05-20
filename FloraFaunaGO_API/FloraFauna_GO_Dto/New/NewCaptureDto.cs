using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.New;

public class NewCaptureDto
{
    public string? Id { get; set; }
    public byte[]? photo { get; set; }

    public LocalisationNormalDto LocalisationNormalDto { get; set; }

    public bool Shiny { get; set; }
}
