namespace ServeyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error InvalidQuestion =
        new("Vote.InvalidQuestions", "Invalid questions", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidAnswer =
        new("Vote.InvalidAnswer", "Invalid answer", StatusCodes.Status400BadRequest);
    public static readonly Error DuplicatedVote =
        new("Vote.Duplicated", "You have already voted in this poll", statusCode: StatusCodes.Status409Conflict);
}
