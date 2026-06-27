namespace ServeyBasket.Contracts.Auth;

public class ResendConfirmEmailValidator : AbstractValidator<ResendConfirmEmailRequest>
{
    public ResendConfirmEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
