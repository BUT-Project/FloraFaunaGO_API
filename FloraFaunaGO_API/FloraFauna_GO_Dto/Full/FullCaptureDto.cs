using FloraFauna_GO_Dto.Normal;
using FloraFaunaGO_Entities.Enum;

namespace FloraFauna_GO_Dto.Full;

public class FullCaptureDto
{
    public CaptureNormalDto capture { get; set; }
    public FullCaptureDetailDto[]? CaptureDetails { get; set; }

    public Famille[]? familles { get; set; }

    public Regime_Alimentaire Regime_Alimentaire { get; set; }
}