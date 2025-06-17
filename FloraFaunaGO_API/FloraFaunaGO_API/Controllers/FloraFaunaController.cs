using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.New;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/")]
public class FloraFaunaController : ControllerBase
{
    private readonly ILogger<FloraFaunaController> _logger;
    private readonly IFileStorageService _fileStorageService;

    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public FloraFaunaController(ILogger<FloraFaunaController> logger, FloraFaunaService service, IFileStorageService fileStorageService)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
        UnitOfWork = service;
    }

    [HttpPost("success/state/idUser={iduser}&idSuccess={idsuccess}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostSuccessState(string idsuccess, string iduser, [FromBody] SuccessStateNormalDto dto)
    {
        if (dto == null) return BadRequest();
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
    public async Task<IActionResult> PostCapture([FromForm] NewCaptureDto dto, string iduser, string idespece)
    {
        if (dto == null) return BadRequest();
        var capture = (await UnitOfWork.CaptureRepository.GetCaptureByEspece(idespece)).Items.Where(c => c.idUtilisateur == iduser).FirstOrDefault();
        if (capture != null)
        {
            var result = await PostCaptureDetail(
                new NewCaptureDetailDto()
                {
                    Localisation = dto.LocalisationNormalDto,
                    CaptureDetail = new CaptureDetailNormalDto() { Shiny = dto.Shiny }
                },
                capture.Capture.Id ?? string.Empty);
            var newcapture = (await UnitOfWork.CaptureRepository.GetCaptureByEspece(idespece)).Items.Where(c => c.idUtilisateur == iduser).FirstOrDefault();
            return Ok(newcapture);
        }
        var user = await UnitOfWork.UserRepository.GetById(iduser);
        // Handle photo upload if provided
        string? photoUrl = null;
        if (dto.photo != null)
        {
            photoUrl = await _fileStorageService.UploadAsync(dto.photo, "captures");
        }
        
        _ = await UnitOfWork.AddCaptureAsync(new CaptureNormalDto() { Id = dto.Id, photoUrl = photoUrl, IdEspece = idespece, LocalisationNormalDto = dto.LocalisationNormalDto, Shiny = dto.Shiny }, user.Utilisateur);
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
        var deleted = await UnitOfWork.DeleteCaptureAsync(new CaptureNormalDto() { Id = capture.Capture.Id, IdEspece = capture.Capture.IdEspece, photoUrl = capture.Capture.photoUrl}, user!.Utilisateur, captureDetails?.Items?.Select(cd => cd.CaptureDetail).ToList() ?? new List<CaptureDetailNormalDto>());
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("capturedetail/idCapture={idCapture}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCaptureDetail([FromBody] NewCaptureDetailDto dto, string idCapture)
    {
        if (dto == null) return BadRequest();
        var capture = await UnitOfWork.CaptureRepository.GetById(idCapture);
        _ = await UnitOfWork.AddCaptureDetailAsync(dto.CaptureDetail, new CaptureNormalDto() { Id = capture.Capture.Id, IdEspece = capture.Capture.IdEspece, photoUrl = capture.Capture.photoUrl }, dto.Localisation);
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
        var deleted = await UnitOfWork.DeleteCaptureDetailAsync(captureDetail.CaptureDetail, new CaptureNormalDto() { Id = capture.Capture.Id, IdEspece = capture.Capture.IdEspece, photoUrl = capture.Capture.photoUrl }, localisation);
        return deleted ? Ok() : NotFound();
    }

    [HttpDelete("utilisateur/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUtilisateur(string id)
    {
        var user = await UnitOfWork.UserRepository.GetById(id);
        if (user == null) return NotFound();

        // Convert ResponseCaptureDto to CaptureNormalDto  
        var captures = user.Capture?.Select(c => new CaptureNormalDto
        {
            Id = c.Id,
            IdEspece = c.IdEspece,
            photoUrl = c.photoUrl
        }).ToList() ?? new List<CaptureNormalDto>();

        // Proceed with SuccessState conversion  
        var successStates = user.SuccessState?.ToList() ?? new List<SuccessStateNormalDto>();

        var deleted = await UnitOfWork.DeleteUser(user.Utilisateur, captures, successStates);
        return deleted ? Ok() : NotFound();
    }

    [HttpPost("espece")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEspece([FromBody] FullEspeceDto dto)
    {
        if (dto == null) return BadRequest();
        _ = await UnitOfWork.AddEspeceAsync(dto, dto.localisations);
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
        espece.localisations = (await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(id)).Items.ToArray();
        var deleted = await UnitOfWork.DeleteEspeceAsync(espece, espece.localisations);
        return deleted ? Ok() : NotFound();
    }

}
