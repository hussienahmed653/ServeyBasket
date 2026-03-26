namespace ServeyBasket.Services;

public class PollServices(ServeyBasketDbContext context) : IPollServices
{
    private readonly ServeyBasketDbContext _context = context;

    public async Task<Poll> AddAsync(Poll poll)
    {
        poll.Id = await _context.Polls.AsNoTracking().AnyAsync() ? await _context.Polls.AsNoTracking().MaxAsync(i => i.Id) + 1 : 1;
        await _context.Polls.AddAsync(poll);
        await _context.SaveChangesAsync();
        return poll;
    }

    public async Task<bool> DeletedAsync(int id)
    {
        var getpoll = await GetAsync(id);
        if (getpoll is null)
            return false;

        _context.Polls.Remove(getpoll);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Poll?> GetAsync(int id) =>
        await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Poll>> GetAllAsync() =>
        await _context.Polls
            .AsNoTracking()
            .ToListAsync();

    public async Task<bool> UpdateAsync(int id, Poll poll)
    {
        var getpoll = await GetAsync(id);
        if (getpoll is null)
            return false;

        getpoll.Title = poll.Title;
        getpoll.Summary = poll.Summary;
        getpoll.StartsAt = poll.StartsAt;
        getpoll.EndsAt = poll.EndsAt;


        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> TogglePublishStatusAsync(int id)
    {
        var poll = await GetAsync(id);
        if (poll is null)
            return false;

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync();

        return true;
    }
}
