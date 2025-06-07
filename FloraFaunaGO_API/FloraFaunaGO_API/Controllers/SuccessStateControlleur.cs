using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
namespace FloraFaunaGO_API.Controllers;


[ApiController]
[Route("FloraFaunaGo_API/success/state/")]
public class SuccessStateControlleur : ControllerBase
{
    private readonly ILogger<SuccessStateControlleur> _logger;
    public ISuccessStateRepository<SuccessStateNormalDto,  FullSuccessStateDto> Repository { get; set; }
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    
    public SuccessStateControlleur(ILogger<SuccessStateControlleur> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        Repository = service.SuccessStateRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullSuccessStateDto>> GetById(string id)
    {
        try
        {
            var result = await Repository.GetById(id);
            if (result == null) return NotFound();
            result.Success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(id)).Items.FirstOrDefault();
            result.User = (await UnitOfWork.UserRepository.GetUserBySuccessState(id)).Items.FirstOrDefault().Utilisateur;
            return result != null ? Ok(result) : NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return NotFound();
    }

    private async Task<ActionResult<Pagination<FullSuccessStateDto>>> GetSuccessStates(Func<Task<Pagination<FullSuccessStateDto>>> func)
    {
        var result = await func();
        foreach (var item in result.Items)
        {
            item.Success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(item.State.Id)).Items.FirstOrDefault();
            item.User = (await UnitOfWork.UserRepository.GetUserBySuccessState(item.State.Id)).Items.First().Utilisateur;
        }
        return result != null ? Ok(result) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullSuccessStateDto>>> GetAll([FromQuery] SuccessStateOrderingCreteria criterium = SuccessStateOrderingCreteria.None,
                                           [FromQuery] int index = 0,
                                           [FromQuery] int count = 10)
    {
        return await GetSuccessStates(async () => await Repository.GetAllSuccessState(criterium, index, count));
    }

    [HttpGet("User={id}&Success{idSuccess}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullSuccessStateDto>> GetByIdUserAndSuccess(string id, string idSuccess)
    {
        try
        {
            var results = await Repository.GetSuccessStateByUser(id);
            foreach (var item in results.Items)
            {
                item.Success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(item.State.Id)).Items.FirstOrDefault();
            }
            if (results == null) return NotFound();
            var result = results.Items.FirstOrDefault(ss => ss.Success.Id == idSuccess);

            result.User = (await UnitOfWork.UserRepository.GetUserBySuccessState(result.State.Id)).Items.FirstOrDefault().Utilisateur;
            return result != null ? Ok(result) : NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullSuccessStateDto>> PutSuccessState(string id,[FromBody] EditSuccessDto dto)
    {
        var tmp = await Repository.GetById(id);
        tmp.Success = (await UnitOfWork.SuccessRepository.GetSuccessBySuccessState(id)).Items.FirstOrDefault();
        if ( tmp is null ) return NoContent();
        var result = await Repository.Update(id, new SuccessStateNormalDto() { Id = id, IsSucces = dto.PercentSucces >= tmp.Success.Objectif, PercentSucces = dto.PercentSucces});
        if (((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutSuccessState), result) : NotFound(id);
    }
}
