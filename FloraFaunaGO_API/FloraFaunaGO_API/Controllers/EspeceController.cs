using FloraFauna_GO_Dto.Full;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/espece/")]
public class EspeceController : ControllerBase
{
    private readonly ILogger<EspeceController> _logger;

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllEspece()
    {
        throw new NotImplementedException();
    }

    [HttpGet("famille/{famille}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFamille(string famille)
    {
        throw new NotImplementedException();
    }

    [HttpGet("regimeAlimentaire/{regime_alimentaire}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByRegimeAlimentaire(string regime_alimentaire)
    {
        throw new NotImplementedException();
    }

    [HttpGet("habitat/{idHabitat}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdHabitat(string idHabitat)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostEspece([FromBody] FullEspeceDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutEspece([FromQuery] string id, [FromBody] FullEspeceDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEspece([FromQuery] string id)
    {
        throw new NotImplementedException();
    }
}