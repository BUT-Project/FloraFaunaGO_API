using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities;

public class UtilisateurEntities : IdentityUser
{
    [Required]
    public DateTime DateInscription { get; set; } = DateTime.Now;

    public ICollection<CaptureEntities>? Captures { get; set; } =  new List<CaptureEntities>();

    public ICollection<SuccesStateEntities>? SuccesState { get; set; } = new List<SuccesStateEntities>();
}
