using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/capturedetail/")]
public class CaptureDetailController : ControllerBase
{
    private readonly ILogger<CaptureDetailController> _logger;
    public ICaptureDetailRepository<CaptureDetailNormalDto, FullCaptureDetailDto> CaptureDetailRepository { get; private set; }
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    public CaptureDetailController(ILogger<CaptureDetailController> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        CaptureDetailRepository = service.CaptureDetailRepository;
    }

    [HttpGet ("id={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await CaptureDetailRepository.GetById(id);
        var localisation = await UnitOfWork.LocalisationRepository.GetById(result.localisationNormalDtos.Id);
        result.localisationNormalDtos = localisation;
        if (result == null) return NotFound();
        return Ok(result);
    }

    private async Task<IActionResult> GetCaptureDetail(Func<Task<Pagination<FullCaptureDetailDto>>> func)
    {
        var result = await func();
        foreach (var item in result.Items)
        {
            item.localisationNormalDtos = await UnitOfWork.LocalisationRepository.GetById(item.localisationNormalDtos.Id);
        }
        return result != null ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCaptureDetail([FromQuery] CaptureDetailOrderingCriteria criterium = CaptureDetailOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetCaptureDetail(async () => await CaptureDetailRepository.GetAllCaptureDetail(criterium, index, count));
    }

    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> PostCaptureDetail([FromBody] FullCaptureDetailDto dto)
    //{
    //    _ = await CaptureDetailRepository.Insert(dto);
    //    var inserted = await UnitOfWork.SaveChangesAsync();

    //    if ((inserted?.Count() ?? -1) != 1) return BadRequest();
    //    var insertedCaptureDetail = inserted.SingleOrDefault();
    //    return insertedCaptureDetail != null ? CreatedAtAction(nameof(PostCaptureDetail),insertedCaptureDetail) : BadRequest();
    //}

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCaptureDetail([FromQuery] string id,[FromBody] CaptureDetailNormalDto dto)
    {
        var result = await CaptureDetailRepository.Update(id, dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutCaptureDetail), result) : NotFound(id);
    }

    //[HttpDelete]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> DeleteCaptureDetail([FromQuery] string id)
    //{
    //    var result = await CaptureDetailRepository.Delete(id);
    //    if (await UnitOfWork.SaveChangesAsync() == null) return NotFound(id);
    //    return result ? Ok() : NotFound(id);
    //}
}
