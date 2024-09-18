using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class CaptureDetails
{
    private Guid id;

    public Guid Id
    {
        get;
    }

    private bool shiny;
    public bool Shiny { get; set; }

    private Localisation localisation;
    public Localisation Localisation
    {
        get => localisation;
        set
        {
            if (value == null || value == localisation) return;
            localisation = value;
        }
    }

    public CaptureDetails(bool shiny, Localisation localisation)
    {
        Shiny = shiny;
        Localisation = localisation;
    }
}
