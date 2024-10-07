using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FloraFaunaGO_Entities.Enum;

namespace FloraFauna_GO_Entities;

public class EspeceEntities : BaseEntity
{
    public string Nom {  get; set; }

    public string Nom_scientifique { get; set; }

    public string? Description { get; set; }
  
    public Blob Image {  get; set; }

    public Blob? Image3D { get; set; }
    public Famille Famille { get; set; }

    public ICollection<HabitatEntities>? Habitats {  get; set; } = new Collection<HabitatEntities>();

    public ICollection<LocalisationEntities>? Localisations { get; set; } = new Collection<LocalisationEntities>();
    public Regime_Alimentaire Regime { get; set; }
    public bool AddHabitat(HabitatEntities item)
    {
        bool found = false;
        Habitats.ToList().ForEach(hab => {
            if (hab.Id == item.Id)
                found = true;
        });
        if (!found) Habitats.Add(item);
        return found;
    }

    public bool RemoveHabitat(HabitatEntities item)
    {
        bool found = false;
        Habitats.ToList().ForEach(hab => {
            if (hab.Id == item.Id)
                found = true;
        });
        if (found) Habitats.Remove(item);
        return found;
    }

    public HabitatEntities? RechHabitat(string Id)
    {
        HabitatEntities? habitat = null;
        Habitats.ToList().ForEach(hab => {
            if (hab.Id == Id)
                habitat = hab;
        });
        return habitat;
    }

    public bool AddLocalisation(LocalisationEntities item)
    {
        bool found = false;
        Localisations.ToList().ForEach(loc => {
            if (loc.Latitude == item.Latitude && loc.Longitude == item.Longitude && loc.Rayon == item.Rayon)
                found = true;
        });
        if (!found) Localisations.Add(item);
        return found;
    }

    public bool RemoveLocalisation(LocalisationEntities item)
    {
        bool found = false;
        Localisations.ToList().ForEach(loc => {
            if (loc.Latitude == item.Latitude && loc.Longitude == item.Longitude && loc.Rayon == item.Rayon)
                    found = true;
        });
        if (found) Localisations.Remove(item);
        return found;
    }
}
