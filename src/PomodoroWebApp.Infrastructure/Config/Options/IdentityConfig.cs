namespace PomodoroWebApp.Infrastructure.Config.Options;

/// <summary>
/// Configuración de identidad para la aplicación.
/// </summary>
public class IdentityConfig

{
    public PasswordConfig Password { get; set; }
    public UserConfig User { get; set; }
    public LockoutConfig Lockout { get; set; }
    public CookieConfig Cookie { get; set; }

}

/// <summary>
/// Configuración de la contraseña.
/// </summary>

public class PasswordConfig
{
    public bool RequireDigit { get; set; } = true;
    public int RequiredLength { get; set; } = 8;
    public bool RequireNonAlphanumeric { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
}

/// <summary>
/// Configuración del usuario.
/// </summary>

public class UserConfig
{
    public bool RequireUniqueEmail { get; set; } = true;
}

/// <summary>
/// Configuración del bloqueo.
/// </summary>
public class LockoutConfig
{
    public int DefaultLockoutTimeInMinutes { get; set; } = 15;
    public int MaxFailedAccessAttempts { get; set; } = 5;
}

/// <summary>
/// Configuración de la cookie.
/// </summary>
public class CookieConfig
{
    public int ExpireTimeInMinutes { get; set; } = 60;
}
