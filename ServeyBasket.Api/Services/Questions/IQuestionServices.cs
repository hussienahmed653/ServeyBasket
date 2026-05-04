using ServeyBasket.Contracts.Questions;

namespace ServeyBasket.Services.Questions;

public interface IQuestionServices
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
}
