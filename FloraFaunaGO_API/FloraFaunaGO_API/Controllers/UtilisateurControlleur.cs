using FloraFauna_GO_Dto;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.New;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FloraFaunaGO_API.Controllers;
[ApiController]
[Route("FloraFaunaGo_API/utilisateur/")]
public class UtilisateurControlleur : ControllerBase
{
    private readonly ILogger<UtilisateurControlleur> _logger;

    // Permet d'accéder au appsettingJson pour avoir la key du token
    private readonly IConfiguration _configuration;

    public IUserRepository<UtilisateurNormalDto, FullUtilisateurDto> UserRepository { get; set; }

    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public UtilisateurControlleur(ILogger<UtilisateurControlleur> logger, FloraFaunaService service, IConfiguration configuration)
    {
        _logger = logger;
        UnitOfWork = service;
        UserRepository = service.UserRepository;
        _configuration = configuration;
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullUtilisateurDto>> GetPlayerById(string id)
    {
        var user = await UserRepository.GetById(id);
        if(user != null) { 
            user.Capture = (await UnitOfWork.CaptureRepository.GetCaptureByUser(user.Utilisateur.Id)).Items.Select(c => c.Capture).ToArray();
            user.SuccessState = (await UnitOfWork.SuccessStateRepository.GetSuccessStateByUser(user.Utilisateur.Id)).Items.Select(ss => ss.State).ToArray();
        }
        return user != null ? Ok(user) : NotFound(id);
    }

    //[HttpGet("{pseudo}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetPlayerByPesudo(string pesudo)
    //{
    //    throw new NotImplementedException();
    //}

    private async Task<ActionResult<Pagination<FullUtilisateurDto>>> GetUsers(Func<Task<Pagination<FullUtilisateurDto>>> func)
    {
        var result = await func();
        foreach(var user in result.Items)
        {
            user.Capture = (await UnitOfWork.CaptureRepository.GetCaptureByUser(user.Utilisateur.Id)).Items.Select(c => c.Capture).ToArray();
            user.SuccessState = (await UnitOfWork.SuccessStateRepository.GetSuccessStateByUser(user.Utilisateur.Id)).Items.Select(ss => ss.State).ToArray();
        }
        return result.Items.Any() ? Ok(result) : NoContent();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<FullUtilisateurDto>>> GetAllPlayer([FromQuery] UserOrderingCriteria criterium = UserOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetUsers(async () => await UserRepository.GetAllUser(UserOrderingCriteria.None, index, count));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<FullUtilisateurDto>> PostPlayer(UtilisateurNormalDto dto)
    {
        _ = await UserRepository.Insert(dto);
        var inserted = await UnitOfWork.SaveChangesAsync();

        if ((inserted?.Count() ?? -1) != 1) return BadRequest();
        var insertedUser = inserted?.SingleOrDefault();
        return insertedUser != null ? Created(nameof(PostPlayer), insertedUser) : NotFound();
    }

    [HttpPut ("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<FullUtilisateurDto>> PutPlayer(string id, [FromBody] UtilisateurNormalDto dto)
    {
        var result = await UserRepository.Update(id, dto);
        if(((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutPlayer), result) : NotFound(id);
    }
}
