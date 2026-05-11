namespace ServeyBasket.Services.Polls;

public interface IPollServices
{
    Task<IEnumerable<PollResponse>> GetAllAsync();
    Task<IEnumerable<PollResponse>> GetCurrentAsync();
    Task<Result<PollResponse>> GetAsync(int id);
    Task<Result<PollResponse>> AddAsync(PollRequest poll);
    Task<Result> UpdateAsync(int id, PollRequest poll);
    Task<Result> DeletedAsync(int id);
    Task<Result> TogglePublishStatusAsync(int id);

}
