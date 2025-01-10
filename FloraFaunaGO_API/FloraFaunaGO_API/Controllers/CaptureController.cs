using FloraFauna_GO_Dto.Full;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/capture/")]
public class CaptureController : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaptureById(string id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{pseudo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCapturesOfPlayer(string pseudo)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostCapture([FromBody] FullCaptureDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutCapture([FromQuery] string id, [FromBody] FullCaptureDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCapture([FromQuery] string id)
    {
        throw new NotImplementedException();
    }
}