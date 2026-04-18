namespace ServeyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email/Password");

    public static readonly Error InvalidToken =
        new("User.InvalidToken", "Invalid Jwt Token");

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid Refresh Token");
}
