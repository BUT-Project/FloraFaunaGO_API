using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Dto.Edit;

public class EditCaptureDto
{
    public string? idEspece { get; set; }
    public IFormFile? photo { get; set; }
}
