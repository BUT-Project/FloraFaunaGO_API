using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities;

public class CaptureDetailsEntities : BaseEntity
{
    public bool Shiny { get; set; }

    public DateTime DateCapture { get; set; }

    public LocalisationEntities Localisation { get; set; }
}
