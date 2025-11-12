using FluentValidation;
using QuizzGenerate.Dto.register;

namespace QuizzGenerate.Validators;

public class RegisterUserValidator: AbstractValidator<RegisterRequestDto>
{
    public RegisterUserValidator()
    {
        //Name
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
        RuleFor(user => user.Name)
            .Length(2, 70)
            .WithMessage("Name is too long.");
        //Last Name
        RuleFor(user => user.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");
        RuleFor(user => user.LastName)
            .Length(2, 70)
            .WithMessage("Last name is too long.");
        //Email
        RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage("Email is not valid.");
        //Password
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
        RuleFor(user => user.Password)
            .Length(8, 50)
            .WithMessage("Password too short or too long, passwrod must be greater than 8 characters.");
    }
}