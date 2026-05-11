using ServeyBasket.Contracts.Questions;

namespace ServeyBasket.Services.Questions;

public interface IQuestionServices
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
    Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvailable(int pollId, string userId);
    Task<Result<QuestionResponse>> Get(int pollId, int questionId);
    Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request);
    Task<Result> ToggleStatusAsync(int pollId, int questionId);
}
