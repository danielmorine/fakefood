using FluentValidation;
using Application.DTOs;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email obrigatório")
            .EmailAddress()
            .WithMessage("Email inválido");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha obrigatória")
            .MinimumLength(6)
            .WithMessage("Senha deve ter no mínimo 6 caracteres")
            .Matches("[A-Z]")
            .WithMessage("Senha deve conter letra maiúscula")
            .Matches("[0-9]")
            .WithMessage("Senha deve conter número");
    }
}