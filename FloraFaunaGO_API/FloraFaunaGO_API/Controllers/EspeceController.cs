using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/espece/")]
public class EspeceController : ControllerBase
{
    private readonly ILogger<EspeceController> _logger;

    public IEspeceRepository<FullEspeceDto,FullEspeceDto> EspeceRepository { get; private set; }
    public IUnitOfWork<FullEspeceDto, FullCaptureDto, FullUtilisateurDto> UnitOfWork { get; private set; }

    public EspeceController(ILogger<EspeceController> logger)
    {
        _logger = logger;
        //EspeceRepository = especeRepository;
    }

    [HttpGet ("id={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var espece = await EspeceRepository.GetById(id);
        return espece != null ? Ok(espece) : NotFound(id);
    }

    [HttpGet("name={name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(string name)
    {
        var espece = await EspeceRepository.GetEspeceByName( EspeceOrderingCriteria.ByNom);
        return espece != null ? Ok(espece) : NotFound(name);
    }

    private async Task<IActionResult> GetEspeces(Func<Task<Pagination<FullEspeceDto>>> func)
    {
        var result = await func();
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllEspece([FromQuery] EspeceOrderingCriteria criterium = EspeceOrderingCriteria.None, 
                                                  [FromQuery] int index = 0, 
                                                  [FromQuery] int count = 10)
    {
        return await GetEspeces(async () => await EspeceRepository.GetAllEspece(EspeceOrderingCriteria.None, 0, 100));
    }

    [HttpGet("famille={famille}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFamille(string famille)
    {
        var espece = await EspeceRepository.GetEspeceByFamile( EspeceOrderingCriteria.ByFamille);
        return espece != null ? Ok(espece) : NotFound(famille);
    }

    [HttpGet("regimeAlimentaire/{regime_alimentaire}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByRegimeAlimentaire(string regimeAlimentaire)
    {
        var espece = await EspeceRepository.GetEspeceByRegime(EspeceOrderingCriteria.ByRegime);
        return espece != null ? Ok(espece) : NotFound(regimeAlimentaire);
    }

    [HttpGet("habitat/{idHabitat}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdHabitat(string idHabitat)
    {
        var espece = await EspeceRepository.GetEspeceByHabitat(EspeceOrderingCriteria.ByHabitat);
        return espece != null ? Ok(espece) : NotFound(idHabitat);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostEspece([FromBody] FullEspeceDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutEspece([FromQuery] string id, [FromBody] FullEspeceDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEspece([FromQuery] string id)
    {
        throw new NotImplementedException();
    }
}
