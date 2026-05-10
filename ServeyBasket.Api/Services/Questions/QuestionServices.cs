using ServeyBasket.Contracts.Questions;

namespace ServeyBasket.Services.Questions;

public class QuestionServices(ServeyBasketDbContext dbContext) : IQuestionServices
{
    private readonly ServeyBasketDbContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId)
    {
        var pollIsExist = await _dbContext.Polls.AnyAsync(p => p.Id == pollId);

        if (!pollIsExist)
            return Result.Failuer<IEnumerable<QuestionResponse>> (PollErrors.PollNotFound);

        var questions = await _dbContext.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Include(q => q.Answers)
            //.Select(q => new QuestionResponse(
            //    q.Id,
            //    q.Content,
            //    q.Answers.Select(a => new AnswerResponse(
            //        a.Id,
            //        a.Content
            //    ))))
            .ProjectToType<QuestionResponse>()
            .ToListAsync();

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }
    public async Task<Result<QuestionResponse>> Get(int pollId, int questionId)
    {
        var question = await _dbContext.Questions
            .Where(q => q.PollId == pollId && q.Id == questionId && q.IsActive)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .SingleOrDefaultAsync();

        if(question is null)
            return Result.Failuer<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(question);
    }
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
    public async Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request)
    {
        var questionIsExist = await _dbContext.Questions.AnyAsync(
            q => q.Id != questionId &&
            q.Content == request.Content &&
            q.PollId == pollId);
        if (questionIsExist)
            return Result.Failuer(QuestionErrors.DuplicatedQuestionContent);

        var question = await _dbContext.Questions
            .Include(q => q.Answers)
            .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId);
        if (question is null)
            return Result.Failuer(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;

        var currentAnswers = question.Answers.Select(a => a.Content).ToList();

        var newAnswersIds = request.Answers.Except(currentAnswers).ToList();

        newAnswersIds.ForEach(a =>
        {
            question.Answers.Add(new Answer { Content = a });
        });

        question.Answers.ToList().ForEach(a =>
        {
            a.IsActive = request.Answers.Contains(a.Content);
        });

        await _dbContext.SaveChangesAsync();

        return Result.Success();

    }
    public async Task<Result> ToggleStatusAsync(int pollId, int questionId)
    {
        var question = await _dbContext.Questions.SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId);
        if (question is null)
            return Result.Failuer(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
    

