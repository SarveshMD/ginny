using _01_TaskAPI.DTOs;
using FluentValidation;

namespace _01_TaskAPI.Validators;

// public record CreateTodoItemDto(
// int Id,
// string Title,
// string Description,
// DateTimeOffset DueAt
// );

public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    public CreateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(256).WithMessage("Title cannot be longer than 256 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");

        RuleFor(x => x.DueAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("DueAt cannot be in the past");
    }
}
