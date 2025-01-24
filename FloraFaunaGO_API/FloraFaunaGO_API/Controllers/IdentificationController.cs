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
    private readonly ILogger<IdentificationController> _logger;
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, FullCaptureDto, FullCaptureDto, FullCaptureDetailDto, FullCaptureDetailDto, FullUtilisateurDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, FullSuccessStateDto, FullSuccessStateDto> UnitOfWork { get; private set; }
    public IdentificationService Service { get; private set; }

    public IdentificationController(ILogger<CaptureController> logger, FloraFaunaService service)
    {
        UnitOfWork = service;
        Service = new IdentificationService(UnitOfWork.EspeceRepository);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AskToIdentifyAPI([FromBody] AnimalIdentifyNormalDto dto)
    {
        var result = await Service.identify(dto);
        return result != null ? Ok(result) : NoContent();
    }
}
