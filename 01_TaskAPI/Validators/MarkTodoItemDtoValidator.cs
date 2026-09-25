using _01_TaskAPI.DTOs;
using FluentValidation;

namespace _01_TaskAPI.Validators;

public class MarkTodoItemDtoValidator : AbstractValidator<MarkTodoItemDto>
{
    public MarkTodoItemDtoValidator()
    {
        RuleFor(x => x.IsCompleted)
            .NotNull();
    }
}
