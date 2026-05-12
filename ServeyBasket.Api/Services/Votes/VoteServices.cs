using ServeyBasket.Contracts.Votes;

namespace ServeyBasket.Services.Votes;

public class VoteServices(ServeyBasketDbContext context) : IVoteServices
{
    private readonly ServeyBasketDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request)
    {
        var pollIsExist = await _context.Polls
                    .AnyAsync(p => p.Id == pollId && p.IsPublished &&
                    p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                    p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow));
        if (!pollIsExist)
            return Result.Failuer(PollErrors.PollNotFound);

        var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId);
        if (hasVote)
            return Result.Failuer(VoteErrors.DuplicatedVote);

        var availableQuestions = await _context.Questions
            .Where(v => v.PollId == pollId && v.IsActive)
            .Select(v => v.Id)
            .ToListAsync();
        if(!request.Answers.Select(a => a.QuestionId).SequenceEqual(availableQuestions))
            return Result.Failuer(VoteErrors.InvalidQuestion);

        var validAnswers = await _context.Answers
            .Where(a => availableQuestions.Contains(a.QuestionsId) && a.IsActive)
            .Select(a => new { a.Id, a.QuestionsId })
            .ToListAsync();

        foreach (var answer in request.Answers)
        {
            var isValid = validAnswers.Any(a =>
                a.Id == answer.AnswerId && a.QuestionsId == answer.QuestionId
                );
            if (!isValid)
                return Result.Failuer(VoteErrors.InvalidAnswer);
        }
        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _context.AddAsync(vote);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}
