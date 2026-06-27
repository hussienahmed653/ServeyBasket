namespace ServeyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email/Password", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidToken =
        new("User.InvalidToken", "Invalid Jwt Token", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid Refresh Token", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Duplicated Email", statusCode: StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
       new("User.EmailNotConfirmed", "Email is not Confirmed", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error EmailAlreadyConfirmed =
       new("User.EmailAlreadyConfirmed", "Email is already confirmed", statusCode: StatusCodes.Status400BadRequest);

    public static readonly Error InvalidCode =
       new("User.InvalidCode", "Invalid Code", statusCode: StatusCodes.Status400BadRequest);

}
