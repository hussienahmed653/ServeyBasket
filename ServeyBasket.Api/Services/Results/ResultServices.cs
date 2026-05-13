namespace ServeyBasket.Services.Results;

public class ResultServices(ServeyBasketDbContext context) : IResultServices
{
    private readonly ServeyBasketDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId)
    {
        var pollVotes = await _context.Polls
            .Where(p => p.Id == pollId)
            .Select(x => new PollVotesResponse(
                    x.Title,
                    x.Votes.Select(v => new VoteResponse(
                        $"{v.User.FirstName} {v.User.LastName}",
                        v.SubmittedOn,
                        v.VoteAnswers.Select(va => new QuestionAnswerResponse(
                            va.Question.Content,
                            va.Answer.Content
                        )
                ))))).SingleOrDefaultAsync();
        return pollVotes is null 
            ? Result.Failuer<PollVotesResponse>(PollErrors.PollNotFound) 
            : Result.Success(pollVotes);
    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId);

        if (!pollIsExists)
            return Result.Failuer<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDay = await _context.Votes
            .Where(x => x.PollId == pollId)
            .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
            .Select(g => new VotesPerDayResponse(
                g.Key.Date,
                g.Count()
            ))
            .ToListAsync();

        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId);

        if (!pollIsExists)
            return Result.Failuer<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

        var votesPerQuestion = await _context.VoteAnswers
            .Where(x => x.Vote.PollId == pollId)
            .Select(x => new VotesPerQuestionResponse(
                x.Question.Content,
                x.Question.Votes
                    .GroupBy(x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content })
                    .Select(g => new VotesPerAnswerResponse(
                        g.Key.AnswerContent,
                        g.Count()
                    ))
            ))
            .ToListAsync();

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
    }
}
