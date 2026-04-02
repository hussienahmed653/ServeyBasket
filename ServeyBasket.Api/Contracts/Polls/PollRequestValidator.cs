namespace ServeyBasket.Contracts.Polls;

public class AuthRequestValidator : AbstractValidator<PollRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Summary)
            .NotEmpty()
            .MaximumLength(1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
            .NotEmpty()
            .GreaterThan(x => x.StartsAt);
    }
}
