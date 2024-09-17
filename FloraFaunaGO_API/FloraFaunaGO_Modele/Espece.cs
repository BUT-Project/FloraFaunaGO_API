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

    private Famille famille;
    public Famille Famille { get; set; }

    private List<uint> lidHabitat;
    public List<uint> LidHabitat
    {
        get { return lidHabitat; }
        set
        {
            if (value == null) return;
            if (lidHabitat == null) lidHabitat = new List<uint>();
            else lidHabitat = value;
        }
    }

    private List<uint> lidLocalisation;
    public List<uint> LidLocalisation
    {
        get { return lidLocalisation; }
        set
        {
            if (value == null) return;
            if (lidLocalisation == null) lidLocalisation = new List<uint>();
            else lidLocalisation = value;
        }
    }

    private Regime_Alimentaire regime;
    public Regime_Alimentaire Regime { get; set; }

    public Espece(uint id,string nom, string nom_scientifique, string description, uint numero, Blob image, Famille famille, List<uint> lidHabitat, List<uint> lidLocalisation, Regime_Alimentaire regime)
    {
        Id = id;
        Nom = nom;
        Nom_scientifique = nom_scientifique;
        Description = description;
        Numero = numero;
        Image = image;
        Famille = famille;
        LidHabitat = lidHabitat;
        LidLocalisation = lidLocalisation;
        Regime = regime;
    }
}
