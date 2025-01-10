using FloraFauna_GO_Dto.Full;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/utilisateur/")]
public class UtilisateurControlleur : ControllerBase
{
    private readonly ILogger<UtilisateurControlleur> _logger;

    public UtilisateurControlleur(ILogger<UtilisateurControlleur> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlayerById(string id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{pseudo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlayerByPesudo(string pesudo)
    {
        throw new NotImplementedException();
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPlayer()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostPlayer(FullUtilisateurDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutPlayer([FromQuery] string id, [FromBody] FullUtilisateurDto dto)
    {
        throw new NotImplementedException();
    }


    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePlayer([FromQuery] string id)
    {
        throw new NotImplementedException();
    }
}