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
    [Required]
    public string Nom {  get; set; }

    public string? Nom_scientifique { get; set; }

    public string? Description { get; set; }
    
    public byte[]? Image {  get; set; }

    public byte[]? Image3D { get; set; }

    public Famille Famille { get; set; }

    public ICollection<HabitatEntities>? Habitats {  get; set; } = new Collection<HabitatEntities>();

    public ICollection<LocalisationEntities>? Localisations { get; set; } = new Collection<LocalisationEntities>();
    
    public Regime_Alimentaire Regime { get; set; }
}
