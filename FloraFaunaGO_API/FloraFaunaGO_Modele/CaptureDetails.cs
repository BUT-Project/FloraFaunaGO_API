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

}
