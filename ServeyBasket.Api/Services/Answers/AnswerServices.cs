namespace ServeyBasket.Services.Answers;

public class AnswerServices(ServeyBasketDbContext dbContext) : IAnswerServices
{
    private readonly ServeyBasketDbContext _dbContext = dbContext;

    public async Task<int> getid()
    {
        return await _dbContext.Answers.AnyAsync() ? await _dbContext.Answers.MaxAsync(a => a.Id) + 1 : 1;
    }
}
