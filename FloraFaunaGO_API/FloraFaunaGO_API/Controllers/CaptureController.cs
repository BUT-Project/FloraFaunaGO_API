using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/capture/")]
public class CaptureController : ControllerBase
{

    private readonly ILogger<CaptureController> _logger;
    private readonly IFileStorageService _fileStorageService;

    public ICaptureRepository<CaptureNormalDto, FullCaptureDto> CaptureRepository { get; private set; }

    public IUserRepository<UtilisateurNormalDto, FullUtilisateurDto> UserRepository { get; set; }

    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public CaptureController(ILogger<CaptureController> logger, FloraFaunaService service, IFileStorageService fileStorageService)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
        UnitOfWork = service;
        CaptureRepository = service.CaptureRepository;
        UserRepository = service.UserRepository;
    }
    /*
        /// <summary>
        /// get Capture by id
        /// </summary>
        /// <param name="id">identifiant de capture</param>
        /// <remarks>Cool</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
    */
    [HttpGet("{id}")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullCaptureDto>> GetCaptureById(string id)
    {
        var capture = await CaptureRepository.GetById(id);
        if (capture != null)
        {
            var cd = await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(capture.Capture.Id, CaptureDetailOrderingCriteria.None);
            capture.CaptureDetails = cd.Items.ToList();
            foreach (var item in capture.CaptureDetails)
            {
                item.localisationNormalDtos = await UnitOfWork.LocalisationRepository.GetById(item.localisationNormalDtos.Id);
            }
        }
        return capture != null ? Ok(capture) : NotFound();
    }

    [HttpGet("idUser={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullCaptureDto>> GetCaptureByUser(string id)
    {
        var user = await UserRepository.GetById(id);
        if (user == null) 
            return NotFound(id);

        var capture = await CaptureRepository.GetCaptureByUser(id, CaptureOrderingCriteria.ByUser);
        if (capture != null && capture.Items.Count() > 0)
        {
            foreach (var item in capture.Items)
            {
                var cd = await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(item.Capture.Id, CaptureDetailOrderingCriteria.None);
                item.CaptureDetails = cd.Items.ToList();
            }
        }
        return capture != null ? Ok(capture) : NotFound();
    }

    private async Task<ActionResult<Pagination<FullCaptureDto>>> GetCapture(Func<Task<Pagination<FullCaptureDto>>> func)
    {
        var result = await func();
        foreach (var item in result.Items)
        {
            var cd = await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(item.Capture.Id, CaptureDetailOrderingCriteria.None);
            item.CaptureDetails = cd.Items.ToList();
            foreach (var captureDetail in item.CaptureDetails)
            {
                captureDetail.localisationNormalDtos = await UnitOfWork.LocalisationRepository.GetById(captureDetail.localisationNormalDtos.Id);
            }
        }
        return result != null ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullCaptureDto>>> GetAllCapture([FromQuery] CaptureOrderingCriteria criterium = CaptureOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetCapture(async () => await CaptureRepository.GetAllCapture(CaptureOrderingCriteria.None, index, count));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<FullCaptureDto>> PutCapture(string id, [FromForm] EditCaptureDto dto)
    {
        try
        {
            var capture = await CaptureRepository.GetById(id);
            if (capture == null) return NotFound(id);

            if (dto.idEspece != null) 
                capture.Capture.IdEspece = dto.idEspece;

            string? photoUrl = capture.Capture.photoUrl; // Keep existing URL
            if (dto.photo != null)
            {
                if (!string.IsNullOrEmpty(capture.Capture.photoUrl))
                {
                    await _fileStorageService.DeleteAsync(capture.Capture.photoUrl);
                }

                photoUrl = await _fileStorageService.UploadAsync(dto.photo, "captures");
            }

            var result = await CaptureRepository.Update(id, new CaptureNormalDto 
            { 
                Id = capture.Capture.Id, 
                IdEspece = dto.idEspece ?? capture.Capture.IdEspece, 
                photoUrl = photoUrl 
            });

            if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) 
                return BadRequest("Failed to update capture");

            return result != null ? Created(nameof(PutCapture), result) : NotFound(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating capture {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating capture");
        }
    }
}
