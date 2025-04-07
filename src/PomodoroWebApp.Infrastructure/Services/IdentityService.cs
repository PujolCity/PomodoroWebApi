using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;
using PomodoroWebApp.Domain.ValidatorMessages;
using PomodoroWebApp.Infrastructure.Config.InitializationExtensions;
using PomodoroWebApp.Infrastructure.Config.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PomodoroWebApp.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<Result<Usuario>> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.Baja)
            return Result<Usuario?>.Fail(AppValidatorMessage.UserNotFoundError());

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Result<Usuario>.Fail(AppValidatorMessage.InvalidCredentialsError());
        return Result<Usuario>.Ok(user);
    }

    public async Task<Result<IdentityResult>> ChangePasswordAsync(Usuario usuario, string currentPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(usuario, currentPassword, newPassword);
        return Result<IdentityResult>.Ok(result);
    }

    public async Task<Result<string>> GenerateJwtTokenAsync(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Email, usuario.Email!),
            new(ClaimTypes.Name, usuario.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(usuario);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtOptions = _configuration.GetOptions<JwtConfig>("JwtConfig");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOptions.ExpiryInMinutes),
            signingCredentials: creds);

        return Result<string>.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<Result<IdentityResult>> RegisterUserAsync(Usuario usuario, string password)
    {
        try
        {
            var result = await _userManager.CreateAsync(usuario, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<IdentityResult>.Fail($"Error al registrar usuario: {errors}");
            }

            return Result<IdentityResult>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<IdentityResult>.Fail($"Exception: {ex.Message}");
        }
    }
}
