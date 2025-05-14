using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.New;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
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
        if (successState == null) return NotFound();
        var user = (await UnitOfWork.UserRepository.GetUserBySuccessState(id)).Items.FirstOrDefault();
        var success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(id)).Items.FirstOrDefault();
        var deleted = await UnitOfWork.DeleteSuccesStateAsync(successState.State, user.Utilisateur, success);
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("capture/idUser={iduser}&idEspece={idespece}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCapture([FromBody] NewCaptureDto dto, string iduser, string idespece)
    {
        if (dto == null) return BadRequest();
        var capture = (await UnitOfWork.CaptureRepository.GetCaptureByEspece(idespece)).Items.Where(c => c.idUtilisateur == iduser).FirstOrDefault();
        if (capture != null)
        {
            return await PostCaptureDetail(
                new NewCaptureDetailDto()
                {
                    Localisation = dto.LocalisationNormalDto,
                    CaptureDetail = new CaptureDetailNormalDto() { Shiny = dto.Shiny }
                },
                capture.Capture.Id ?? string.Empty);
        }
        var user = await UnitOfWork.UserRepository.GetById(iduser);
        _ = await UnitOfWork.AddCaptureAsync(new CaptureNormalDto() { Id = dto.Id, photo = dto.photo, IdEspece = idespece, LocalisationNormalDto = dto.LocalisationNormalDto }, user.Utilisateur);
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
        var user = await UnitOfWork.UserRepository.GetById(capture.idUtilisateur);
        var captureDetails = await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(id);
        var deleted = await UnitOfWork.DeleteCaptureAsync(capture.Capture, user!.Utilisateur, captureDetails?.Items?.Select(cd => cd.CaptureDetail).ToList() ?? new List<CaptureDetailNormalDto>());
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("capturedetail/idCapture={idCapture}")]
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
        var capture = (await UnitOfWork.CaptureRepository.GetCaptureByCaptureDetail(id)).Items.FirstOrDefault();
        var localisation = (await UnitOfWork.LocalisationRepository.GetLocalisationByCaptureDetail(id)).Items.FirstOrDefault();
        var deleted = await UnitOfWork.DeleteCaptureDetailAsync(captureDetail.CaptureDetail,capture.Capture, localisation);
        return deleted ? Ok() : NotFound();
    }

    [HttpDelete("utilisateur/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUtilisateur(string id)
    {
        var user = await UnitOfWork.UserRepository.GetById(id);
        if (user == null) return NotFound();
        user.Capture = (await UnitOfWork.CaptureRepository.GetCaptureByUser(id)).Items.Select(c => c.Capture).ToArray();
        user.SuccessState = (await UnitOfWork.SuccessStateRepository.GetSuccessStateByUser(id)).Items.Select(s => s.State).ToArray();
        var deleted = await UnitOfWork.DeleteUser(user.Utilisateur, user.Capture.ToList(), user.SuccessState.ToList());
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
        if (espece == null) return NotFound();
        espece.localisationNormalDtos = (await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(id)).Items.ToArray();
        var deleted = await UnitOfWork.DeleteEspeceAsync(espece.Espece, espece.localisationNormalDtos);
        return deleted ? Ok() : NotFound();
    }

}
