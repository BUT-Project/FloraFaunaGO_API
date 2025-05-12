using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/success/")]
public class SuccessControlleur : ControllerBase
{
    private readonly ILogger<SuccessControlleur> _logger;
    public ISuccessRepository<SuccessNormalDto, SuccessNormalDto> SuccessRepository;
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    
    public SuccessControlleur(ILogger<SuccessControlleur> logger, FloraFaunaService service) {
        _logger = logger;
        UnitOfWork = service;
        SuccessRepository = service.SuccessRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await SuccessRepository.GetById(id);
        return result != null ? Ok(result) : NotFound(id);
    }

    private async Task<IActionResult> GetSuccess(Func<Task<Pagination<SuccessNormalDto>>> func)
    {
        var result = await func();
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSuccess([FromQuery] SuccessOrderingCreteria criterium = SuccessOrderingCreteria.None,
                                                   [FromQuery] int index = 0,
                                                   [FromQuery] int count = 10)
    {
        return await GetSuccess(async () => await SuccessRepository.GetAllSuccess(criterium, index, count));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostSuccess([FromBody] SuccessNormalDto dto)
    {
        var Toto = await SuccessRepository.Insert(dto);
        var inserted = await UnitOfWork.SaveChangesAsync();

        if ((inserted?.Count() ?? -1) != 1) return BadRequest();
        var insertedSuccess = inserted.SingleOrDefault();
        return insertedSuccess != null ? CreatedAtAction(nameof(PostSuccess), insertedSuccess) : BadRequest();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutSuccess(string id,[FromBody] SuccessNormalDto dto)
    {
        var result = await SuccessRepository.Update(id, dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutSuccess), result) : NotFound();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSuccess(string id)
    {
        bool result = await SuccessRepository.Delete(id);
        if (await UnitOfWork.SaveChangesAsync() == null) return NotFound(id);
        return result ? Ok() : NotFound();
    }
}
