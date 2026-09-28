using _01_TaskAPI.DTOs;
using FluentValidation;

namespace _01_TaskAPI.Validators;

public class TaskQueryParametersValidator : AbstractValidator<TaskQueryParameters>
{
    public TaskQueryParametersValidator()
    {
        RuleFor(x => x.page)
            .GreaterThanOrEqualTo(1);

        // TODO: add dueAfter <= dueBefore rule when both are specified
        // otherwise the query's redundant (always empty)

        RuleFor(x => x.pageSize)
            .InclusiveBetween(1, 10).WithMessage("pageSize must be between 1 and 10");
    }
}
