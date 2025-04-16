using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;
using PomodoroWebApp.Domain.ValidatorMessages;
using PomodoroWebApp.Infrastructure.Config.InitializationExtensions;
using PomodoroWebApp.Infrastructure.Config.Options;
using PomodoroWebApp.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PomodoroWebApp.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly PomodoroDbContext _dbContext;
    private readonly JwtConfig _jwtConfig;

    public IdentityService(UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IConfiguration configuration,
        PomodoroDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _jwtConfig = _configuration.GetOptions<JwtConfig>("JwtConfig");
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

    public async Task<Result<AuthResponse>> GenerateJwtTokenAsync(Usuario usuario)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Name, usuario.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(usuario);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = usuario.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays),
                Used = false,
                Invalidated = false
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return Result<AuthResponse>.Ok(new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                TokenExpiry = token.ValidTo
            });
        }
        catch (Exception ex)
        {
            return Result<AuthResponse>.Fail($"Error al generar token: {ex.Message}");
        }
    }


    public async Task<Result<AuthResponse>> RefreshTokenAsync(string jwtToken, string refreshToken)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(jwtToken, _jwtConfig.Secret);
            if (principal == null)
                return Result<AuthResponse>.Fail("Token inválido o malformado");

            // 2. Validar JTI (ID del token JWT)
            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
                return Result<AuthResponse>.Fail("El token no contiene un JTI válido");

            var storedRefreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedRefreshToken == null || storedRefreshToken.IsExpired || storedRefreshToken.Invalidated)
                return Result<AuthResponse>.Fail("Refresh token inválido o expirado");

            if (storedRefreshToken.JwtId != jti)
                return Result<AuthResponse>.Fail("El refresh token no corresponde a este JWT");

            var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result<AuthResponse>.Fail("Usuario no encontrado");

            storedRefreshToken.Used = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            return await GenerateJwtTokenAsync(user);
        }
        catch (Exception ex)
        {
            return Result<AuthResponse>.Fail($"Error al refrescar token: {ex.Message}");
        }
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string secret)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = _jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfig.Audience,
            ValidateLifetime = false, 
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
