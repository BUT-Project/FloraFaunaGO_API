using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class Localisation
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

    private double latitude;
    public double Latitude { get; set; }

    private double longitude;
    public double Longitude { get; set; }

    private double rayon;
    public double Rayon
    {
        get { return rayon; }
        set
        {
            if (value < 0) rayon = 0;
            else rayon = value;
        }
    }
}
