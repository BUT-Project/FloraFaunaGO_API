using System.ComponentModel.DataAnnotations;

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
