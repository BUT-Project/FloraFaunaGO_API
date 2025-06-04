using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/espece/")]
public class EspeceController : ControllerBase
{
    private readonly ILogger<EspeceController> _logger;

    public IEspeceRepository<EspeceNormalDto,FullEspeceDto> EspeceRepository { get; private set; }
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public EspeceController(ILogger<EspeceController> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        EspeceRepository = UnitOfWork.EspeceRepository;
    }

    [HttpGet ("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEspeceDto>> GetById(string id)
    {
        var espece = await EspeceRepository.GetById(id);
        var localisations = await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(id);
        if (espece != null)
            espece.localisationNormalDtos = localisations.Items.ToArray();

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
            var localisations = await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(especes.Items.First().Espece.Id);
            especes.Items.First().localisationNormalDtos = localisations.Items.ToArray();
            espece = especes.Items.First();
        }
        return espece != null ? Ok(espece) : NotFound(name);
    }

    private async Task<ActionResult<Pagination<FullEspeceDto>>> GetEspeces(Func<Task<Pagination<FullEspeceDto>>> func)
    {
        var result = await func();
        foreach (var item in result.Items)
        {
            item.localisationNormalDtos = (await UnitOfWork.LocalisationRepository.GetLocalisationByEspece(item.Espece.Id)).Items.ToArray();
            for (int i = 0; i < item.localisationNormalDtos.Length; i++)
                item.localisationNormalDtos[i] = await UnitOfWork.LocalisationRepository.GetById(item.localisationNormalDtos[i].Id);

        }
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullEspeceDto>>> GetAllEspece([FromQuery] EspeceOrderingCriteria criterium = EspeceOrderingCriteria.None, 
                                                  [FromQuery] int index = 0, 
                                                  [FromQuery] int count = 10)
    {
        return await GetEspeces(async () => await EspeceRepository.GetAllEspece(EspeceOrderingCriteria.None, index, count));
    }

    [HttpGet("famille={famille}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullEspeceDto>>> GetByFamille(string famille)
    {
        var espece = await EspeceRepository.GetEspeceByFamile( EspeceOrderingCriteria.ByFamille);
        return espece != null ? Ok(espece) : NotFound(famille);
    }

    [HttpGet("regimeAlimentaire={regime_alimentaire}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullEspeceDto>>> GetByRegimeAlimentaire(string regimeAlimentaire)
    {
        var espece = await EspeceRepository.GetEspeceByRegime(EspeceOrderingCriteria.ByRegime);
        return espece != null ? Ok(espece) : NotFound(regimeAlimentaire);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<FullEspeceDto>> PutEspece(string id, [FromBody] EspeceNormalDto dto)
    {
        var result = await EspeceRepository.Update(id,dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutEspece), result) : NotFound(id);
    }
}
