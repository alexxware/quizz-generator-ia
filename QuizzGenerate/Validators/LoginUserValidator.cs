using FluentValidation;
using QuizzGenerate.Dto.login;

namespace QuizzGenerate.Validators;

public class LoginUserValidator: AbstractValidator<LoginRequestDto>
{
    public LoginUserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}