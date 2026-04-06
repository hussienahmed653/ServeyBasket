using Microsoft.AspNetCore.Identity.Data;

namespace ServeyBasket.Contracts.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
