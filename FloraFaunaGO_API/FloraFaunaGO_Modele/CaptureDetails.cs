using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class CaptureDetails
{
    private uint id;

    public uint Id
    {
        get { return id; }
        set
        {
            if (value < 0) id = 0;
            else id = value;
        }
    }

    private bool shiny;
    public bool Shiny { get; set; }

    private uint idLocalisation;
    public uint IdLocalisation
    {
        get { return idLocalisation; }
        set
        {
            if (value < 0) idLocalisation = 0;
            else idLocalisation = value;
        }
    }

    public CaptureDetails(uint id, bool shiny, uint idLocalisation)
    {
        Id = id;
        Shiny = shiny;
        IdLocalisation = idLocalisation;
    }
}
