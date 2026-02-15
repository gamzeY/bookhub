using backend.Dtos;
using FluentValidation;

namespace backend.Validation;

public sealed class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(BookRules.TitleMax);

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(BookRules.AuthorMax);

        RuleFor(x => x.Isbn)
            .NotEmpty().WithMessage("ISBN is required.")
            .MaximumLength(BookRules.IsbnMax);

        RuleFor(x => x.CoverImageUrl)
            .MaximumLength(BookRules.CoverUrlMax);

        RuleFor(x => x.Rating)
            .Must(r => r is null or >= 1 and <= 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comments)
            .MaximumLength(BookRules.CommentsMax);

        When(x => x.Rating is not null, () =>
        {
            RuleFor(x => x.Comments)
                .NotEmpty().WithMessage("Comments are required when rating is provided.");
        });

        RuleFor(x => x.Comments)
            .Must(c => !BookRules.ContainsForbiddenWord(c))
            .WithMessage("Comments must not contain the word 'horrible'.");
    }
}
