using FluentValidation;
using ServeyBasket.Contracts.Requests;

namespace ServeyBasket.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}
