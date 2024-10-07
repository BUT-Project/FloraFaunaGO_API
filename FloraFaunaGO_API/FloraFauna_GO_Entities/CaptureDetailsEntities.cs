using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class CaptureDetailsEntities
{
    private Guid id;

    public Guid Id
    {
        get;
    }

    private bool shiny;
    public bool Shiny { get; set; }

    private LocalisationEntities localisation;
    public LocalisationEntities Localisation
    {
        get => localisation;
        set
        {
            if (value == null || value == localisation) return;
            localisation = value;
        }
    }

    public CaptureDetailsEntities() { }
}
