namespace ServeyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote =
        new("Vote.Duplicated", "You have already voted in this poll", statusCode: StatusCodes.Status409Conflict);
}
