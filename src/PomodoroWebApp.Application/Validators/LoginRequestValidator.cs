using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace PomodoroWebApp.Application.Validators;

/// <summary>
/// Validador para la solicitud de inicio de sesión.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("Email no válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.");
    }
}