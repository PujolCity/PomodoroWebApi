using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Repositories;

namespace PomodoroWebApp.Application.Validators;

/// <summary>
/// Validador para la solicitud de registro de un nuevo usuario.
/// </summary>
public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
{
    private readonly UserManager<Usuario> _userManager;


    public RegisterRequestDTOValidator(UserManager<Usuario> userManager)
    {
        _userManager = userManager;

        // Validación para Nombre
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

        // Validación para Apellido
        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.");

        // Validación para Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email es requerido")
            .EmailAddress().WithMessage("Formato de email inválido")
            .Must(BeAValidEmailDomain).WithMessage("El dominio del email no es válido.")
            .MustAsync(async (email, _) => (await _userManager.FindByEmailAsync(email)) == null)
            .WithMessage("El email ya está registrado");

        // Validación para NombreUsuario
        RuleFor(x => x.NombreUsuario)
            .NotEmpty().WithMessage("Nombre de usuario es requerido")
            .MinimumLength(4).WithMessage("El nombre de usuario debe tener al menos 4 caracteres.")
            .MustAsync(async (username, _) => (await _userManager.FindByNameAsync(username)) == null)
            .WithMessage("El nombre de usuario ya está en uso");

        // Validación para Password
        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial (ej: !@#$%^&*).");
        _userManager = userManager;
    }

    // Método opcional para validar el dominio del email (ejemplo: rechazar dominios temporales)
    private bool BeAValidEmailDomain(string email)
    {
        var domainsToBlock = new[] { "temp.com", "fake.org" }; // Ejemplo: lista de dominios no permitidos
        var domain = email.Split('@').LastOrDefault();
        return domain != null && !domainsToBlock.Contains(domain);
    }
}
