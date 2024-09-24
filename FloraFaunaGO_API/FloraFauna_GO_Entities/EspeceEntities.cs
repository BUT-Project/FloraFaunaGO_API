using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FloraFaunaGO_Modele.Enum;

namespace FloraFaunaGO_Modele;

public class EspeceEntities
{
    private Guid id;

    public Guid Id { get; }

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

    

    private Blob image;
    public Blob Image
    {
        get { return image; }
        set
        {
            if (Equals(value, null)) image = default;
            else image = value;
        }
    }

    private Famille famille;
    public Famille Famille { get; set; }

    private List<HabitatEntities> habitats;
    public List<HabitatEntities> Habitats => habitats;

    private List<LocalisationEntities> localisations;
    public List<LocalisationEntities> Localisations => localisations;
    

    private Regime_Alimentaire regime;
    public Regime_Alimentaire Regime { get; set; }

    public EspeceEntities()
    {
    }

    public EspeceEntities(string nom, string nom_scientifique, string description, uint numero, Blob image, Famille famille, Regime_Alimentaire regime)
    {
        Nom = nom;
        Nom_scientifique = nom_scientifique;
        Description = description;
        Numero = numero;
        Image = image;
        Famille = famille;
        habitats = new List<HabitatEntities>();
        localisations = new List<LocalisationEntities>();
        Regime = regime;
    }
}
