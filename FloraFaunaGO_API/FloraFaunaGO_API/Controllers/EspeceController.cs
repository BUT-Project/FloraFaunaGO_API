using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/espece/")]
public class EspeceController : ControllerBase
{
    private readonly ILogger<EspeceController> _logger;

    public IEspeceRepository<FullEspeceDto, FullEspeceDto> EspeceRepository { get; private set; }
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public EspeceController(ILogger<EspeceController> logger, FloraFaunaService service)
    {
        _logger = logger;
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
                Image = item.Image,
                Image3D = item.Image3D,
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
    public async Task<ActionResult<FullEspeceDto>> PutEspece(string id, [FromBody] EditEspeceDto dto)
    {
        var espece = new FullEspeceDto
        {
            Id = id,
            Nom = dto.Nom,
            Famille = dto.Famille,
            Regime = dto.Regime,
            Image = dto.Image,
            Image3D = dto.Image3D,
            Description = dto.Description
        };
        var result = await EspeceRepository.Update(id, espece);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutEspece), result) : NotFound(id);
    }
}
