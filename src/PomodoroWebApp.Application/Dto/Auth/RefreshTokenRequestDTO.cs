﻿namespace PomodoroWebApp.Application.Dto.Auth;

public class RefreshTokenRequestDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
