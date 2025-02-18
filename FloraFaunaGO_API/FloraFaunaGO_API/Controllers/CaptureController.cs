
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/capture/")]
public class CaptureController : ControllerBase
{

    private readonly ILogger<CaptureController> _logger;

    public ICaptureRepository<CaptureNormalDto, FullCaptureDto> CaptureRepository { get; private set; }

    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public CaptureController(ILogger<CaptureController> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        CaptureRepository = service.CaptureRepository;
    }

    /// <summary>
    /// get Capture by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet ("id={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaptureById(string id)
    {
        var capture = await CaptureRepository.GetById(id);
        return capture != null ? Ok(capture) : NotFound();
    }

    [HttpGet ("user={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaptureByUser(string id)
    {
        throw new NotImplementedException();
    }

    private async Task<IActionResult> GetCapture(Func<Task<Pagination<FullCaptureDto>>> func)
    {
        var result = await func();
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCapture([FromQuery] CaptureOrderingCriteria criterium = CaptureOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetCapture(async () => await CaptureRepository.GetAllCapture(CaptureOrderingCriteria.None, index, count));
    }

    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<IActionResult> PostCapture([FromBody] FullCaptureDto dto)
    //{
    //    var capture = await CaptureRepository.Insert(dto);
    //    var inserted = await UnitOfWork.SaveChangesAsync();

    //    if ((inserted?.Count() ?? -1) != 1) return BadRequest();
    //    var insertedCapture = inserted?.SingleOrDefault();
    //    return insertedCapture != null ? CreatedAtAction(nameof(PostCapture), insertedCapture) : BadRequest();
    //}

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutCapture([FromQuery] string id, [FromBody] CaptureNormalDto dto)
    {
        var result = await CaptureRepository.Update(id, dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutCapture), result) : NotFound(id);
    }

    //[HttpDelete]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> DeleteCapture([FromQuery] string id)
    //{
    //    bool result = await CaptureRepository.Delete(id);
    //    if(await  UnitOfWork.SaveChangesAsync() == null) return NotFound(id);
    //    return result ? Ok(result) : NotFound(id);
    //}
}
