using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.Full;

public class FullSuccessStateDto
{
    public SuccessStateNormalDto State { get; set; }
    public SuccessNormalDto Success { get; set; }

    public UtilisateurNormalDto User { get; set; }
}
