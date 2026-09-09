using FluentValidation;
using _00_BookAPI.DTOs;

namespace _00_BookAPI.Validators;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(256).WithMessage("Max title length is 256 characters.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Max author length is 100 characters.");

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year)
            .WithMessage($"Published Year must be between 1000 and {DateTime.UtcNow.Year}.");
    }
}
