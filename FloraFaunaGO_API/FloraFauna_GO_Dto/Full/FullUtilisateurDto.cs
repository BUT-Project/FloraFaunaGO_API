using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.Full;

public class FullUtilisateurDto
{
    public UtilisateurNormalDto Utilisateur { get; set; }
    public ResponseCaptureDto[]? Capture { get; set; }

    public SuccessStateNormalDto[]? SuccessState { get; set; }
}
