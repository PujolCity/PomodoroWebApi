namespace PomodoroWebApp.Domain.Entities;

/// <summary>
/// Clase que representa un token de actualización.
/// </summary>
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Used { get; set; }
    public bool Invalidated { get; set; }
    public int UserId { get; set; }
    public Usuario User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => !Used && !Invalidated && !IsExpired;
}