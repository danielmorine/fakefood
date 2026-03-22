using Application.DTOs;
using Application.Interfaces;
using AuthService.Application.Interfaces;
using AuthService.Common;
using AuthService.Common.Result;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IValidator<LoginRequest> _validator;
    private readonly ICorrelationIdProvider _correlation;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUseCase(IUserRepository userRepository, ITokenService tokenService, IValidator<LoginRequest> validator, ICorrelationIdProvider correlation, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _validator = validator;
        _correlation = correlation;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> ExecuteAsync(LoginRequest request)
    {
        return await ResultExecutor.ExecuteAsync(async () =>
        {
            var registerValidate = await _validator.ValidateAsync(request);

            if (!registerValidate.IsValid)
                throw new Exception(string.Join(", ", registerValidate.Errors.Select(e => e.ErrorMessage)));

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new Exception("Erro ao tentar autenticar, verifique email e senha");

            return _tokenService.GenerateToken(user.Email);
        }, _correlation.Get());
    }
}
