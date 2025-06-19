using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Dto.Normal;

public class UtilisateurNormalDto
{
    public string? Id { get; set; }
    public string? Pseudo { get; set; }

    public string? ImageUrl { get; set; }
    
    public string? Mail { get; set; }

    public string Hash_mdp { get; set; }

    public DateTime DateInscription { get; set; }
}