using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities;

public class LocalisationEntities : BaseEntity
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }
    public double Rayon { get; set; }

    [Required]
    public string EspeceId { get; set; }

    public EspeceEntities Espece { get; set; }

    [Required]
    public string CaptureDetailsId { get; set; }

    public CaptureDetailsEntities CaptureDetails { get; set; }

}
