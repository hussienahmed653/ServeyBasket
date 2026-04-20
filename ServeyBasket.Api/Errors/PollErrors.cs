namespace ServeyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found", statusCode: StatusCodes.Status404NotFound);
}
