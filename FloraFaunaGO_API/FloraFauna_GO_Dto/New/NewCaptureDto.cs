using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.New;

public class NewCaptureDto
{
    public string? Id { get; set; }
    public byte[]? photo { get; set; }

    public LocalisationNormalDto LocalisationNormalDto { get; set; }

    public bool Shiny { get; set; }
}
