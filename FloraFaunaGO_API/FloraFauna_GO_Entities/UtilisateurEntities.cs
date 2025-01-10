using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities;

public class UtilisateurEntities : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Pseudo { get; set; }

    [EmailAddress]
    [MaxLength (70)]
    public string Mail { get; set; }

    [Required]
    [MinLength(8)]
    public string Hash_mdp { get; set; }

    [Required]
    public DateTime DateInscription { get; set; }

    public ICollection<CaptureEntities>? Captures { get; set; } =  new Collection<CaptureEntities>();

    public ICollection<SuccesStateEntities>? SuccesState { get; set; } = new Collection<SuccesStateEntities>();
}
