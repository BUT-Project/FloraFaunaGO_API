using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("FloraFaunaGo_API/success/state/")]
public class SuccessStateControlleur : ControllerBase
{
    private readonly ILogger<SuccessStateControlleur> _logger;
    public ISuccessStateRepository<SuccessStateNormalDto,  FullSuccessStateDto> Repository { get; set; }
    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    
    public SuccessStateControlleur(ILogger<SuccessStateControlleur> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        Repository = service.SuccessStateRepository;
    }

    [HttpGet("idSuccess={idSuccess}&&idUser={idUser}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string idSuccess, string idUser)
    {
        try
        {
            var result = await Repository.GetSuccessStateByUser_Success(idUser, idSuccess);
            return result != null ? Ok(result) : NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return NotFound();
    }

    private async Task<IActionResult> GetSuccessStates(Func<Task<Pagination<FullSuccessStateDto>>> func)
    {
        var result = await func();
        foreach (var item in result.Items)
        {
            item.Success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(item.State.Id)).Items.FirstOrDefault();
            item.User = (await UnitOfWork.UserRepository.GetUserBySuccessState(item.State.Id)).Items.First().Utilisateur;
        }
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

    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> PostSuccessState([FromBody] FullSuccessStateDto dto)
    //{
    //    _ = await Repository.Insert(dto);
    //    var inserted = await UnitOfWork.SaveChangesAsync();

    //    if((inserted?.Count() ?? -1) != 1) return BadRequest();
    //    var insertedSuccessState = inserted?.SingleOrDefault();
    //    return insertedSuccessState != null ? Created(nameof(PostSuccessState), insertedSuccessState) : BadRequest();
    //}

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutSuccessState([FromQuery] string id,[FromBody] SuccessStateNormalDto dto)
    {
        var result = await Repository.Update(id, dto);
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutSuccessState), result) : NotFound(id);
    }

    //[HttpDelete]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> DeleteSuccessState([FromQuery] string id)
    //{
    //    bool result = await Repository.Delete(id);
    //    if (await UnitOfWork.SaveChangesAsync() == null) return NotFound(id);
    //    return result ? Ok() : NotFound(id);
    //}
}
