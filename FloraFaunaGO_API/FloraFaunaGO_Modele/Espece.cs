using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFaunaGO_Modele;

public class Espece
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

    private string nom;

    public string Nom
    {
        get { return nom; }
        set
        {
            if (string.IsNullOrEmpty(value)) nom = "default";
            else nom = value;
        }
    }

    private string nom_scientifique;

    public string Nom_scientifique
    {
        get { return nom_scientifique; }
        set
        {
            if (string.IsNullOrEmpty(value)) nom_scientifique = "default";
            else nom_scientifique = value;
        }
    }

    private string description;

    public string Description
    {
        get { return description; }
        set
        {
            if (value == null) description = "";
            else description = value;
        }
    }

    private uint numero;
    public uint Numero
    {
        get { return numero; }
        set
        {
            if (value < 0) numero = 0;
            else numero = value;
        }
    }

    private Blob image;
    public Blob Image
    {
        get { return image; }
        set
        {
            if (Blob.Equals(value, null)) image = default;
            else image = value;
        }
    }
}
