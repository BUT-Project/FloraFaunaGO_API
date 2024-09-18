using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class Habitat
{
    private uint id;
    public uint Id
    {
        get { return id; }
        set { }
    }

    private string zone;
    public string Zone
    {
        get { return zone; }
        set
        {
            if (string.IsNullOrEmpty(value)) zone = "Zone";
            else zone = value;
        }
    }

    private string climat;
    public string Climat
    {
        get { return climat; }
        set
        {
            if (string.IsNullOrEmpty(value)) climat = "Climat";
            else climat = value;
        }
    }

    public Habitat(string zone,  string climat, uint id)
    {
        Id = id;
        Zone = zone;
        Climat = climat;
    }
}
