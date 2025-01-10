using FloraFauna_GO_Dto.Normal;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;
[ApiController]
[Route("FloraFaunaGo_API/identification")]
public class IdentificationController : ControllerBase
{
    private readonly ILogger<IdentificationController> _logger;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AskToIdentifyAPI([FromBody] AnimalIdentifyNormalDto dto)
    {
        throw new NotImplementedException();
        //return un espece Dto
    }
}
