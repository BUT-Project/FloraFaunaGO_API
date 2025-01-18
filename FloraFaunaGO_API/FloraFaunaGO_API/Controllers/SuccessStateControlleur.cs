using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;
namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/success/state/")]
public class SuccessStateControlleur : ControllerBase
{
    private readonly ILogger<SuccessStateControlleur> _logger;
    public ISuccessStateRepository<FullSuccessStateDto,  FullSuccessStateDto> Repository { get; set; }
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, FullCaptureDto, FullCaptureDto, FullCaptureDetailDto, FullCaptureDetailDto, FullUtilisateurDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, FullSuccessStateDto, FullSuccessStateDto> UnitOfWork { get; private set; }
    
    public SuccessStateControlleur(ILogger<SuccessStateControlleur> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        Repository = service.SuccessStateRepository;
    }

    [HttpGet("id={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await Repository.GetById(id);
        return result != null ? Ok(result) : NotFound(id);
    }

    private async Task<IActionResult> GetSuccessStates(Func<Task<Pagination<FullSuccessStateDto>>> func)
    {
        var result = await func();
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] SuccessStateOrderingCreteria criterium = SuccessStateOrderingCreteria.None,
                                           [FromQuery] int index = 0,
                                           [FromQuery] int count = 10)
    {
        return await GetSuccessStates(async () => await Repository.GetAllSuccessState(criterium, index, count));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostSuccessState([FromBody] FullSuccessStateDto dto)
    {
        _ = await Repository.Insert(dto);
        var inserted = await UnitOfWork.SaveChangesAsync();

        if((inserted?.Count() ?? -1) != 1) return BadRequest();
        var insertedSuccessState = inserted?.SingleOrDefault();
        return insertedSuccessState != null ? Created(nameof(PostSuccessState), insertedSuccessState) : BadRequest();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutSuccessState([FromQuery] string id,[FromBody] FullSuccessStateDto dto)
    {
        var result = await Repository.Update(id, dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutSuccessState), result) : NotFound(id);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSuccessState([FromQuery] string id)
    {
        bool result = await Repository.Delete(id);
        if (await UnitOfWork.SaveChangesAsync() == null) return NotFound(id);
        return result ? Ok() : NotFound(id);
    }
}
