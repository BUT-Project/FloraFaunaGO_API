using System.ComponentModel.DataAnnotations;

namespace FloraFauna_GO_Entities;

public class EspeceEntities : BaseEntity
{
    [Required]
    public string Nom { get; set; }

    public string? Nom_scientifique { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public byte[]? Image3D { get; set; }

    public string? Famille { get; set; }

    public string? Zone { get; set; }

    public string? Climat { get; set; }

    public string? Class { get; set; }

    public string? Kingdom { get; set; }

    public ICollection<EspeceLocalisationEntities> Localisations { get; set; } = new List<EspeceLocalisationEntities>();

    public ICollection<CaptureEntities> Captures { get; set; } = new List<CaptureEntities>();

    public string? Regime { get; set; }
}

public enum Class
{
    Mammals,
    Birds,
    Reptiles,
    Amphibians,
    Fish,
    Insects,
    Arachnids,
    Mollusks,
    Crustaceans,
    Angiosperms
}

public enum Kingdom
{
    Animal,
    Plant,
    Fungi,
    Protista
}
