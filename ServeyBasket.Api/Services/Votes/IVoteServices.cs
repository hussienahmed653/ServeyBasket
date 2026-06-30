namespace ServeyBasket.Services.Votes;

public interface IVoteServices
{
    Task<Result> AddAsync(int pollId, string userId, VoteRequest request);
}
