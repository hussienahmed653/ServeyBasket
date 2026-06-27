namespace ServeyBasket.Contracts.Auth;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
