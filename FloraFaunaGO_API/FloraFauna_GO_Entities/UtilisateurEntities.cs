using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FloraFauna_GO_Entities;

public class UtilisateurEntities : IdentityUser
{
    public string? ImageUrl { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [Required]
    public DateTime DateInscription { get; set; } = DateTime.Now;

    public ICollection<CaptureEntities>? Captures { get; set; } = new List<CaptureEntities>();

    public ICollection<SuccesStateEntities>? SuccesState { get; set; } = new List<SuccesStateEntities>();
}
