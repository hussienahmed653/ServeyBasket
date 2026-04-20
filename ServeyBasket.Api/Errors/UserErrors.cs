namespace ServeyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email/Password", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidToken =
        new("User.InvalidToken", "Invalid Jwt Token", statusCode: StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid Refresh Token", statusCode: StatusCodes.Status401Unauthorized);
}
