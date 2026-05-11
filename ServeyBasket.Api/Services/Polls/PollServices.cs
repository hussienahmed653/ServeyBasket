namespace ServeyBasket.Services.Polls;

public class PollServices(ServeyBasketDbContext context) : IPollServices
{
    private readonly ServeyBasketDbContext _context = context;

    public async Task<Result<PollResponse>> AddAsync(PollRequest pollreq)
    {
        var isExisting = await _context.Polls.AsNoTracking().AnyAsync(p => p.Title == pollreq.Title);
        if (isExisting)
            return Result.Failuer<PollResponse>(PollErrors.DublicatedPollTitle);

        //var Id = await _context.Polls.AsNoTracking().AnyAsync() ? await _context.Polls.AsNoTracking().MaxAsync(i => i.Id) + 1: 1;
        var poll = pollreq.Adapt<Poll>();
        //poll.Id = Id;

        await _context.Polls.AddAsync(poll);
        await _context.SaveChangesAsync();

        return Result.Success(poll.Adapt<PollResponse>());
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
    public async Task<IEnumerable<PollResponse>> GetAllAsync() => 
        await _context.Polls
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync();

    public async Task<IEnumerable<PollResponse>> GetCurrentAsync() =>
        await _context.Polls
            .Where(p => p.IsPublished &&
                    p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                    p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync();
    public async Task<Result> UpdateAsync(int id, PollRequest request)
    {
        var isExisting = await _context.Polls.AsNoTracking().AnyAsync(p => p.Title == request.Title && p.Id != id);
        if (isExisting)
            return Result.Failuer<PollResponse>(PollErrors.DublicatedPollTitle);

        var getpoll = await _context.Polls.FindAsync(id);
        if (getpoll is null)
            return Result.Failuer(PollErrors.PollNotFound);

        getpoll.Title = request.Title;
        getpoll.Summary = request.Summary;
        getpoll.StartsAt = request.StartsAt;
        getpoll.EndsAt = request.EndsAt;


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
