using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.Full;

public class FullUtilisateurDto
{
    public UtilisateurNormalDto Utilisateur { get; set; }
    public CaptureNormalDto[]? Capture { get; set; }

    public SuccessNormalDto[]? Success { get; set; }
}