using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FloraFauna_GO_Entities;

public class UtilisateurEntities : BaseEntity
{
    [Required] [MaxLength(50)] public string Pseudo { get; set; }

    [EmailAddress] [MaxLength(100)] public string Mail { get; set; }

    [Required] [MinLength(8)] public string Hash_mdp { get; set; }

    public DateTime DateInscription { get; set; }

    public ICollection<CaptureEntities>? Captures { get; set; } = new Collection<CaptureEntities>();
}