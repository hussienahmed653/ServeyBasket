namespace ServeyBasket.Services.Polls;

public interface IPollServices
{
    Task<Result<IEnumerable<PollResponse>>> GetAllAsync();
    Task<Result<PollResponse>> GetAsync(int id);
    Task<Result<PollResponse>> AddAsync(PollRequest poll);
    Task<Result> UpdateAsync(int id, PollRequest poll);
    Task<Result> DeletedAsync(int id);
    Task<Result> TogglePublishStatusAsync(int id);

}
