using Application.DTOs;
using Application.Interfaces;
using AuthService.Application.Interfaces;
using AuthService.Common;
using AuthService.Common.Result.Base;
using Domain.Entities;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases;

public class RegisterUseCase
{
    private readonly IUserRepository _repository;
    private readonly IValidator<LoginRequest> _validator;
    private readonly ICorrelationIdProvider _correlation;
    private readonly IPasswordHasher _passwordHasher;


    public RegisterUseCase(IUserRepository repository, IValidator<LoginRequest> validator, ICorrelationIdProvider correlation, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _validator = validator;
        _correlation = correlation;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> ExecuteAsync(LoginRequest request)
    {
        return await ResultExecutor.ExecuteAsync(async () =>
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));

            var userExists = await _repository.GetByEmailAsync(request.Email);

            if (userExists is not null)
                throw new Exception("Não foi possível criar usuário");

            var user = new User(request.Email, _passwordHasher.Hash(request.Password));
            await _repository.AddAsync(user);
        }, 
        _correlation.Get());
    }
}
