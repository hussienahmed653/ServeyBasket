namespace ServeyBasket.Services.Results;

public interface IResultServices
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId);
    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId);
}
