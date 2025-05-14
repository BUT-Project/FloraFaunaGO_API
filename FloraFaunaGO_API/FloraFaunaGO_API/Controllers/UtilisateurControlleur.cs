using FloraFauna_GO_Dto;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.New;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.AspNetCore.Mvc;
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

    public IUserRepository<UtilisateurNormalDto, FullUtilisateurDto> UserRepository { get; set; }

    public IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

    public UtilisateurControlleur(ILogger<UtilisateurControlleur> logger, FloraFaunaService service)
    {
        _logger = logger;
        UnitOfWork = service;
        UserRepository = service.UserRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlayerById(string id)
    {
        var user = await UserRepository.GetById(id);
        if(user != null) { 
            user.Capture = (await UnitOfWork.CaptureRepository.GetCaptureByUser(user.Utilisateur.Id)).Items.Select(c => c.Capture).ToArray();
            user.SuccessState = (await UnitOfWork.SuccessStateRepository.GetSuccessStateByUser(user.Utilisateur.Id)).Items.Select(ss => ss.State).ToArray();
        }
        return user != null ? Ok(user) : NotFound(id);
    }

    [HttpGet("{pseudo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlayerByPesudo(string pesudo)
    {
        throw new NotImplementedException();
    }

    private async Task<IActionResult> GetUsers(Func<Task<Pagination<FullUtilisateurDto>>> func)
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPlayer([FromQuery] UserOrderingCriteria criterium = UserOrderingCriteria.None,
                                                  [FromQuery] int index = 0,
                                                  [FromQuery] int count = 10)
    {
        return await GetUsers(async () => await UserRepository.GetAllUser(UserOrderingCriteria.None, index, count));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostPlayer(UtilisateurNormalDto dto)
    {
        _ = await UserRepository.Insert(dto);
        var inserted = await UnitOfWork.SaveChangesAsync();

        if ((inserted?.Count() ?? -1) != 1) return BadRequest();
        var insertedUser = inserted?.SingleOrDefault();
        return insertedUser != null ? Created(nameof(PostPlayer), insertedUser) : NotFound();
    }

    [HttpPut ("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutPlayer([FromQuery] string id, [FromBody] UtilisateurNormalDto dto)
    {
        var result = await UserRepository.Update(id, dto);
        if(((await UnitOfWork.SaveChangesAsync())?.Count() ?? 0) == 0) return BadRequest();
        return result != null ? Created(nameof(PutPlayer), result) : NotFound(id);
    }

    [HttpPut("login")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = (await UserRepository.GetUserByMail(dto.Mail)).Items.FirstOrDefault();
        var hash = dto.Password; // TODO: hash password

        if (user == null || user.Utilisateur.Hash_mdp != hash) return NotFound();

        var token = GenerateJwtToken(user.Utilisateur.Id, user.Utilisateur.Mail);
        return Ok(new { Token = token });
    }

    [HttpPut("logout")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }

    [HttpPut("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] NewUtilisateurDto dto)
    {
        if (dto == null) return BadRequest();
        if ((await UserRepository.GetAllUser()).Items.Any(u => u.Utilisateur.Mail == dto.Mail || u.Utilisateur.Pseudo == dto.Pseudo))
            return BadRequest("Identifiant already use");

        var user = new UtilisateurNormalDto()
        {
            Mail = dto.Mail,
            Pseudo = dto.Pseudo,
            Hash_mdp = dto.password, // TODO: hash password
            DateInscription = DateTime.Now,
        };

        var result = await PostPlayer(user);
        if (result is CreatedResult)
        {
            var token = GenerateJwtToken(user.Id, user.Mail);
            return Ok(new { Token = token });
        }

        return BadRequest();
    }

    private string GenerateJwtToken(string userId, string userEmail)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("votre-cle-secrete"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, userEmail),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: "FloraFaunaIssuer",
            audience: "FloraFaunaAudience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
