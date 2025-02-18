using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities;

public class CaptureDetailsEntities : BaseEntity
{
    public bool Shiny { get; set; } = false;

    [Required]
    public DateTime DateCapture { get; set; }

    [Required]
    public LocalisationEntities Localisation { get; set; }

    public string LocalisationId { get; set; }

    public CaptureEntities Capture { get; set; }

    [Required]
    public string CaptureId { get; set; }
}
