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
[Route("FloraFaunaGo_API/espece/")]
public class EspeceController : ControllerBase
{
    private readonly ILogger<EspeceController> _logger;
    private readonly IFileStorageService _fileStorageService;

    public IEspeceRepository<FullEspeceDto, FullEspeceDto> EspeceRepository { get; private set; }
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public EspeceController(ILogger<EspeceController> logger, FloraFaunaService service, IFileStorageService fileStorageService)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
        UnitOfWork = service;
        EspeceRepository = UnitOfWork.EspeceRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEspeceDto>> GetById(string id)
    {
        var espece = await EspeceRepository.GetById(id);
        var localisations = await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(id);
        if (espece != null)
            espece.localisations = localisations.Items.ToArray();

        return espece != null ? Ok(espece) : NotFound(id);
    }

    [HttpGet("name={name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEspeceDto>> GetByName(string name)
    {
        var especes = await EspeceRepository.GetEspeceByName(name, EspeceOrderingCriteria.ByNom);
        FullEspeceDto? espece = null;
        if (especes != null && especes.Items.Count() > 0)
        {
            var localisations = await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(especes.Items.First().Id);
            especes.Items.First().localisations = localisations.Items.ToArray();
            espece = especes.Items.First();
        }
        return espece != null ? Ok(espece) : NotFound(name);
    }

    private async Task<ActionResult<Pagination<EspeceNormalDto>>> GetEspeces(Func<Task<Pagination<FullEspeceDto>>> func)
    {
        var result = await func();
        var returned = new Pagination<EspeceNormalDto>()
        {
            Count = result.Count,
            Index = result.Index,
            Total = result.Total,
        };
        var list = new List<EspeceNormalDto>();
        foreach (var item in result.Items)
        {
            var espece = new EspeceNormalDto
            {
                Id = item.Id,
                Nom = item.Nom,
                ImageUrl = item.ImageUrl,
                Image3DUrl = item.Image3DUrl,
            };
            list.Add(espece);

        }
        returned.Items = list;
        return returned != null ? Ok(returned) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<EspeceNormalDto>>> GetAllEspece([FromQuery] EspeceOrderingCriteria criterium = EspeceOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetEspeces(async () => await EspeceRepository.GetAllEspece(criterium, index, count));
    }

    [HttpGet("{id}/filtered={property}&value={value}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<EspeceNormalDto>>> GetAllEspeceByCateg(
                                                string id,
                                                string? property,
                                                string? value,
                                                [FromQuery] int index = 0,
                                                [FromQuery] int count = 10)
    {
        var criterium = EspeceOrderingCriteria.None;
        if (!Enum.TryParse(property, true, out criterium))
            return BadRequest("Action invalide.");
        return await GetEspeces(async () => await EspeceRepository.GetEspeceByProperty(id, value, criterium, index, count));
    }

    [HttpGet("famille={famille}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullEspeceDto>>> GetByFamille(string famille, [FromQuery] EspeceOrderingCriteria criterium = EspeceOrderingCriteria.ByFamille,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        var espece = await EspeceRepository.GetEspeceByFamile(criterium, index, count);
        return espece != null ? Ok(espece) : NotFound(famille);
    }

    [HttpGet("regimeAlimentaire={regime_alimentaire}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullEspeceDto>>> GetByRegimeAlimentaire(string regimeAlimentaire, [FromQuery] EspeceOrderingCriteria criterium = EspeceOrderingCriteria.ByRegime,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        var espece = await EspeceRepository.GetEspeceByRegime(criterium, index, count);
        return espece != null ? Ok(espece) : NotFound(regimeAlimentaire);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FullEspeceDto>> PutEspece(string id, [FromForm] EditEspeceDto dto)
    {
        try
        {
            var existingEspece = await EspeceRepository.GetById(id);
            if (existingEspece == null) return NotFound(id);

            string? imageUrl = existingEspece.ImageUrl; // Keep existing URL
            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(existingEspece.ImageUrl))
                {
                    await _fileStorageService.DeleteAsync(existingEspece.ImageUrl);
                }

                imageUrl = await _fileStorageService.UploadAsync(dto.Image, "especes");
            }

            string? image3DUrl = existingEspece.Image3DUrl; // Keep existing URL
            if (dto.Image3D != null)
            {
                if (!string.IsNullOrEmpty(existingEspece.Image3DUrl))
                {
                    await _fileStorageService.DeleteAsync(existingEspece.Image3DUrl);
                }

                image3DUrl = await _fileStorageService.UploadAsync(dto.Image3D, "especes/3d");
            }

            var espece = new FullEspeceDto
            {
                Id = id,
                Nom = dto.Nom ?? existingEspece.Nom,
                Famille = dto.Famille ?? existingEspece.Famille,
                Regime = dto.Regime ?? existingEspece.Regime,
                ImageUrl = imageUrl,
                Image3DUrl = image3DUrl,
                Description = dto.Description ?? existingEspece.Description
            };

            var result = await EspeceRepository.Update(id, espece);
            if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) 
                return BadRequest("Failed to update espece");

            return result != null ? Created(nameof(PutEspece), result) : NotFound(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating espece {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating espece");
        }
    }
}
