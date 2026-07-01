namespace ServeyBasket.Contracts.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Length(3, 256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexFormate.Password)
            .WithMessage("Password Should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(x => x.Roles)
            .NotNull()
            .NotEmpty()
            .WithMessage("Invalid role(s) provided.");

        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot provide duplicate roles.");
    }
}
