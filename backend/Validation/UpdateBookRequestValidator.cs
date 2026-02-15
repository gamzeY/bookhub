using backend.Dtos;
using FluentValidation;

namespace backend.Validation;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
    }
}
