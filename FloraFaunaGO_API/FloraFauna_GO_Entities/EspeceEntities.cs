using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FloraFaunaGO_Modele.Enum;
using static System.Net.Mime.MediaTypeNames;

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

    public bool AddHabitat(HabitatEntities item)
    {
        bool found = false;
        Habitats.ForEach(hab => {
            if (hab.Id == item.Id)
                found = true;
        });
        if (!found) Habitats.Add(item);
        return found;
    }

    public bool RemoveHabitat(HabitatEntities item)
    {
        bool found = false;
        Habitats.ForEach(hab => {
            if (hab.Id == item.Id)
                found = true;
        });
        if (found) Habitats.Remove(item);
        return found;
    }

    public HabitatEntities? RechHabitat(Guid Id)
    {
        HabitatEntities? habitat = null;
        Habitats.ForEach(hab => {
            if (hab.Id == Id)
                habitat = hab;
        });
        return habitat;
    }

    public bool AddLocalisation(LocalisationEntities item)
    {
        bool found = false;
        Localisations.ForEach(loc => {
            if (loc.Latitude == item.Latitude && loc.Longitude == item.Longitude && loc.Rayon == item.Rayon)
                found = true;
        });
        if (!found) Localisations.Add(item);
        return found;
    }

    public bool RemoveLocalisation(LocalisationEntities item)
    {
        bool found = false;
        Localisations.ForEach(loc => {
            if (loc.Latitude == item.Latitude && loc.Longitude == item.Longitude && loc.Rayon == item.Rayon)
                    found = true;
        });
        if (found) Localisations.Remove(item);
        return found;
    }
}
