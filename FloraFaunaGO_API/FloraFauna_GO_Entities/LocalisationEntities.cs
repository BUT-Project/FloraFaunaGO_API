using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public ICollection<EspeceLocalisationEntities>? EspeceLocalisation { get; set; } = new List<EspeceLocalisationEntities>();
    public CaptureDetailsEntities? CapturesDetail { get; set; }
    public string? CaptureDetailsId { get; set; }
}
