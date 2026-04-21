namespace ServeyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found", statusCode: StatusCodes.Status404NotFound);

    public static readonly Error DublicatedPollTitle =
        new("Poll.DublicatedTitle", "There is another poll with the same title", statusCode: StatusCodes.Status409Conflict);
}
