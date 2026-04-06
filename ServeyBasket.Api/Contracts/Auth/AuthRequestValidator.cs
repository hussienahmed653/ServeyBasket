namespace ServeyBasket.Contracts.Auth;

public class RefreshTokenValidator : AbstractValidator<AuthRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
