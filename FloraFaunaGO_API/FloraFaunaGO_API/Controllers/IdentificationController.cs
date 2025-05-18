using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;
[ApiController]
[Route("FloraFaunaGo_API/identification")]
public class IdentificationController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    private readonly ILogger<IdentificationController> _logger;
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    public IdentificationService Service { get; private set; }

    public IdentificationController(ILogger<CaptureController> logger, FloraFaunaService service, IWebHostEnvironment env)
    {
        UnitOfWork = service;
        Service = new IdentificationService(UnitOfWork.EspeceRepository);
        _env = env;
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

    [HttpPost("classify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClassifyImage([FromBody] AnimalIdentifyNormalDto dto)
    {
        var result = await Service.classify(dto.AskedImage);
        return result is EspeceType.None ? Ok(result) : NoContent();
    }
}
