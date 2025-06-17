using FloraFauna_GO_Dto.Normal;
using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Dto.New;

public class NewCaptureDto
{
    public string? Id { get; set; }
    public IFormFile? photo { get; set; }

    public LocalisationNormalDto LocalisationNormalDto { get; set; }

    public bool Shiny { get; set; }
}
