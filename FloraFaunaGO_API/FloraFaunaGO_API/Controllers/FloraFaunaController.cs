using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.New;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/")]
public class FloraFaunaController : ControllerBase
{
    private readonly ILogger<FloraFaunaController> _logger;

    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public FloraFaunaController(ILogger<FloraFaunaController> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
    }

    [HttpPost("success/state/idUser={iduser}&idSuccess={idsuccess}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostSuccessState(string idsuccess, string iduser, [FromBody] SuccessStateNormalDto dto)
    {
        if(dto == null) return BadRequest();
        var user = await UnitOfWork.UserRepository.GetById(iduser);
        var success = await UnitOfWork.SuccessRepository.GetById(idsuccess);
        _ = await UnitOfWork.AddSuccesStateAsync(dto, user.Utilisateur, success);
        var inserted = await UnitOfWork.SaveChangesAsync();
        if ((inserted?.Count() ?? 0) == 0) return BadRequest();
        var insertedState = inserted;
        return insertedState != null ? CreatedAtAction(nameof(PostSuccessState), insertedState) : BadRequest();
    }

    [HttpDelete("success/state/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSuccessState(string id)
    {
        var successState = await UnitOfWork.SuccessStateRepository.GetById(id);
        var user = await UnitOfWork.UserRepository.GetById(successState.State.ToEntities().UtilisateurId);
        var success = await UnitOfWork.SuccessRepository.GetById(successState!.Success.Id!);
        var deleted = await UnitOfWork.DeleteSuccesStateAsync(successState.State, user.Utilisateur, success);
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("capture/idUser={iduser}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCapture([FromBody] CaptureNormalDto dto, string iduser)
    {
        if (dto == null) return BadRequest();
        var user = await UnitOfWork.UserRepository.GetById(iduser);
        _ = await UnitOfWork.AddCaptureAsync(dto, user.Utilisateur);
        var inserted = await UnitOfWork.SaveChangesAsync();
        if ((inserted?.Count() ?? 0) == 0) return BadRequest();
        var insertedCapture = inserted;
        return insertedCapture != null ? CreatedAtAction(nameof(PostCapture), insertedCapture) 
            : BadRequest();
    }

    [HttpDelete("capture/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCapture(string id)
    {
        var capture = await UnitOfWork.CaptureRepository.GetById(id);
        var user = await UnitOfWork.UserRepository.GetById(capture.Utilisateur.Utilisateur.ToEntities().Id);
        var deleted = await UnitOfWork.DeleteCaptureAsync(capture.Capture, user.Utilisateur, capture.CaptureDetails.Select(cd => cd.CaptureDetail).ToArray());
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("capturedetail/idSuccess={idCapture}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCaptureDetail([FromBody] NewCaptureDetailDto dto, string idCapture)
    {
        if (dto == null) return BadRequest();
        var capture = await UnitOfWork.CaptureRepository.GetById(idCapture);
        _ = await UnitOfWork.AddCaptureDetailAsync(dto.CaptureDetail, capture.Capture, dto.Localisation);
        var inserted = await UnitOfWork.SaveChangesAsync();
        if ((inserted?.Count() ?? 0) == 0) return BadRequest();
        var insertedCaptureDetail = inserted;
        return insertedCaptureDetail != null ? CreatedAtAction(nameof(PostCaptureDetail), insertedCaptureDetail) : BadRequest();
    }

    [HttpDelete("capturedetail/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCaptureDetail(string id)
    {
        var captureDetail = await UnitOfWork.CaptureDetailRepository.GetById(id);
        var capture = await UnitOfWork.CaptureRepository.GetById(captureDetail.CaptureDetail.ToEntities().CaptureId);
        var localisation = await UnitOfWork.LocalisationRepository.GetById(captureDetail.CaptureDetail.ToEntities().LocalisationId);
        var deleted = await UnitOfWork.DeleteCaptureDetailAsync(captureDetail.CaptureDetail,capture.Capture, localisation);
        return deleted ? Ok() : NotFound();
    }

    [HttpDelete("utilisateur/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUtilisateur(string id)
    {
        var user = await UnitOfWork.UserRepository.GetById(id);
        var deleted = await UnitOfWork.DeleteUser(user.Utilisateur, user.Capture.Select(cd => cd.Capture).ToArray(), user.SuccessState.Select(success => success.State).ToArray());
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("espece")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEspece([FromBody] NewEspeceDto dto)
    {
        if (dto == null) return BadRequest();
        _ = await UnitOfWork.AddEspeceAsync(dto.espece, dto.localisation);
        var inserted = await UnitOfWork.SaveChangesAsync();
        if ((inserted?.Count() ?? 0) == 0) return BadRequest();
        var insertedEspece = inserted;
        return insertedEspece != null ? CreatedAtAction(nameof(PostEspece), insertedEspece) : BadRequest();
    }

    [HttpDelete("espece/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEspece(string id)
    {
        var espece = await UnitOfWork.EspeceRepository.GetById(id);
        var deleted = await UnitOfWork.DeleteEspeceAsync(espece.Espece, espece.localisationNormalDtos);
        return deleted ? Ok() : NotFound();
    }

}
