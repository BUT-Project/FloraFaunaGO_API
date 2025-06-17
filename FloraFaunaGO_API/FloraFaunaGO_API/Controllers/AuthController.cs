using FloraFauna_GO_Dto;
using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FloraFauna_GO_Shared.Interfaces;

namespace FloraFaunaGO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UtilisateurEntities> _userManager;
        private readonly IConfiguration _config;

        public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }

        private readonly IFileStorageService _fileStorageService;

        public AuthController(UserManager<UtilisateurEntities> userManager, IConfiguration config, FloraFaunaService service, IFileStorageService fileStorageService)
        {
            _userManager = userManager;
            _config = config;
            UnitOfWork = service;
            _fileStorageService = fileStorageService;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Mail);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Email and/or password is wrong.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                tokenType = "Bearer",
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresIn = 3600,
                refreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest("passwords is not the same.");

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return Conflict("An account with the same email already exist.");

            var user = new UtilisateurEntities
            {
                UserName = dto.Email,
                Email = dto.Email,
                DateInscription = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await CreateUserState(user.Id);

            return Ok("Register succesfully.");
        }

        private async Task CreateUserState(string userId)
        {
            var successes = await UnitOfWork.SuccessRepository.GetAllSuccess(count: 1000);
            if (successes is null) return;
            foreach (var success in successes.Items)
            {
                var _ = UnitOfWork.AddSuccesStateAsync(
                    new SuccessStateNormalDto() { PercentSucces = 0, IsSucces = false },
                    (await UnitOfWork.UserRepository.GetById(userId)).Utilisateur,
                    success
                );
                await UnitOfWork.SaveChangesAsync();
            }

        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Refresh token invalide ou expiré.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                tokenType = "Bearer",
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresIn = 3600,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null) return Ok("if an account existe, the email will be sent.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            Console.WriteLine($"Reset Token : {token}");

            return Ok(new { resetToken = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Mail);
            if (user == null) return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password reset succesfully");
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password updated succesfully");
        }

        [HttpPost("edit-user")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromForm] EditUserDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            user.Email = dto.Mail;
            user.UserName = dto.Pseudo;
            
            if (dto.Image != null)
            {
                
                user.ImageUrl = await _fileStorageService.UploadAsync(dto.Image, "users");
            }
            await _userManager.UpdateAsync(user);

            return Ok(user.ToDto());
        }

    }
}
