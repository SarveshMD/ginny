using _01_TaskAPI.DTOs;
using _01_TaskAPI.Models;
using FluentValidation;
using FluentValidation.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(255).WithMessage("Email address cannot be longer than 255 characters");
    }
}
