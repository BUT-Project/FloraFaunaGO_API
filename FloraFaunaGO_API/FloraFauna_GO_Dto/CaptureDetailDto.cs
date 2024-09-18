using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto;

public class CaptureDetailDto
{
    public Guid Id { get; set; }

    public bool Shiny { get; set; }

    public LocalisationDto Localisation { get; set; }
}
