using ServeyBasket.Contracts.Questions;

namespace ServeyBasket.Services.Questions;

public interface IQuestionServices
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
    Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId);
    Task<Result<QuestionResponse>> Get(int pollId, int questionId);
    Task<Result> ToggleStatusAsync(int pollId, int questionId);
}
