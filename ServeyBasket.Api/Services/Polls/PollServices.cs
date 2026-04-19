namespace ServeyBasket.Services.Polls;

public class PollServices(ServeyBasketDbContext context) : IPollServices
{
    private readonly ServeyBasketDbContext _context = context;

    public async Task<PollResponse> AddAsync(PollRequest pollreq)
    {
        var Id = await _context.Polls.AsNoTracking().AnyAsync() ? await _context.Polls.AsNoTracking().MaxAsync(i => i.Id) + 1: 1;
        var poll = pollreq.Adapt<Poll>();
        poll.Id = Id;
        await _context.Polls.AddAsync(poll);
        await _context.SaveChangesAsync();
        return poll.Adapt<PollResponse>();
    }

    public async Task<Result> DeletedAsync(int id)
    {
        var getpoll = await _context.Polls.FindAsync(id);
        if (getpoll is null)
            return Result.Failuer(PollErrors.PollNotFound);

        _context.Polls.Remove(getpoll);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PollResponse>> GetAsync(int id)
    {
        var poll = await _context.Polls.FindAsync(id);
        if (poll is null)
            return Result.Failuer<PollResponse>(PollErrors.PollNotFound);

        return Result.Success(poll.Adapt<PollResponse>());
    }
    public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync()
    {
        var polls = await _context.Polls
            .AsNoTracking()
            .ToListAsync();
        if (polls is null)
            return Result.Failuer<IEnumerable<PollResponse>>(PollErrors.PollNotFound);

        return Result.Success(polls.Adapt<IEnumerable<PollResponse>>());
    }

    public async Task<Result> UpdateAsync(int id, PollRequest poll)
    {
        var getpoll = await _context.Polls.FindAsync(id);
        if (getpoll is null)
            return Result.Failuer(PollErrors.PollNotFound);

        getpoll.Title = poll.Title;
        getpoll.Summary = poll.Summary;
        getpoll.StartsAt = poll.StartsAt;
        getpoll.EndsAt = poll.EndsAt;


        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id)
    {
        var poll = await _context.Polls.FindAsync(id);
        if (poll is null)
            return Result.Failuer(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
