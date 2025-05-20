using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/identification")]
public class IdentificationController : ControllerBase
{
    private readonly ILogger<IdentificationController> _logger;
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    public IdentificationService Service { get; private set; }

    public IdentificationController(ILogger<CaptureController> logger, FloraFaunaService service)
    {
        UnitOfWork = service;
        Service = new IdentificationService(UnitOfWork.EspeceRepository);
    }

    [HttpPost("{especeType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AskToIdentifyAPI(string especeType, [FromBody] AnimalIdentifyNormalDto dto)
    {
        if (!Enum.TryParse(especeType, true, out EspeceType type))
            return BadRequest("Action invalide.");

        var result = await Service.identify(dto, type);
        return result != null ? Ok(result) : NoContent();
    }
}
