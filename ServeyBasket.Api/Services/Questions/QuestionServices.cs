using ServeyBasket.Contracts.Questions;
using ServeyBasket.Services.Answers;

namespace ServeyBasket.Services.Questions;

public class QuestionServices(ServeyBasketDbContext dbContext) : IQuestionServices
{
    private readonly ServeyBasketDbContext _dbContext = dbContext;

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request)
    {
        var pollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == pollId);

        if (!pollIsExist)
            return Result.Failuer<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExist = await _dbContext.Questions.AnyAsync(q => (q.Content == request.Content) && q.PollId == pollId);

        if (questionIsExist)
            return Result.Failuer<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _dbContext.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        return Result.Success(question.Adapt<QuestionResponse>());
    }
}
    

