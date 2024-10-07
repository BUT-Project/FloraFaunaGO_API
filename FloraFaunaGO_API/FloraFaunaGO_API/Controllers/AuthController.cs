using FloraFauna_GO_Dto.Normal;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;
[ApiController]
[Route("FloraFaunaGo_API/authentification/")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AskToAuthAPI([FromBody] AnimalAuthNormalDto dto)
    {
        throw new NotImplementedException();
        //return un espece Dto
    }
}
