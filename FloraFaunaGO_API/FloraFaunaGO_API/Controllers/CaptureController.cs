
using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

    [HttpGet ("user={id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaptureByUser(string id)
    {
        var capture = await CaptureRepository.GetCaptureByUser(id, CaptureOrderingCriteria.ByUser);
        if (capture != null && !capture.Items.IsNullOrEmpty())
        {
            foreach (var item in capture.Items)
            {
                var cd = await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(item.Capture.Id, CaptureDetailOrderingCriteria.None);
                item.CaptureDetails = cd.Items.ToList();
                foreach (var captureDetail in item.CaptureDetails)
                {
                    captureDetail.localisationNormalDtos = await UnitOfWork.LocalisationRepository.GetById(captureDetail.localisationNormalDtos.Id);
                }
            }
        }
        return capture != null ? Ok(capture) : NotFound();
    }

    private async Task<IActionResult> GetCapture(Func<Task<Pagination<FullCaptureDto>>> func)
    {
        var result = await func();
        foreach(var item in result.Items)
        {
            item.CaptureDetails = (await UnitOfWork.CaptureDetailRepository.GetCaptureDetailByCapture(item.Capture.Id, CaptureDetailOrderingCriteria.None)).Items.ToList();
        }
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
    public async Task<IActionResult> PutCapture([FromQuery] string id, [FromBody] EditCaptureDto dto)
    {
        var capture = await CaptureRepository.GetById(id);
        if (dto.idEspece != null) capture.Capture.IdEspece = dto.idEspece;
        if (dto.photo != null) capture.Capture.photo = dto.photo;
        var result = await CaptureRepository.Update(id, capture.Capture);
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
