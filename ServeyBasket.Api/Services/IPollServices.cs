namespace ServeyBasket.Services;

public interface IPollServices
{
    Task<IEnumerable<Poll>> GetAllAsync();
    Task<Poll?> GetAsync(int id);
    Task<Poll> AddAsync(Poll poll);
    Task<bool> UpdateAsync(int id, Poll poll);
    Task<bool> DeletedAsync(int id);
    Task<bool> TogglePublishStatusAsync(int id);

}
