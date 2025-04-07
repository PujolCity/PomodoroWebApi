namespace PomodoroWebApp.Infrastructure.Config.Options;

public class IdentityConfig

{
    public PasswordConfig Password { get; set; }
    public UserConfig User { get; set; }
    public LockoutConfig Lockout { get; set; }
    public CookieConfig Cookie { get; set; }

}

public class PasswordConfig
{
    public bool RequireDigit { get; set; } = true;
    public int RequiredLength { get; set; } = 8;
    public bool RequireNonAlphanumeric { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
}

public class UserConfig
{
    public bool RequireUniqueEmail { get; set; } = true;
}

public class LockoutConfig
{
    public int DefaultLockoutTimeInMinutes { get; set; } = 15;
    public int MaxFailedAccessAttempts { get; set; } = 5;
}

public class CookieConfig
{
    public int ExpireTimeInMinutes { get; set; } = 60;
}
