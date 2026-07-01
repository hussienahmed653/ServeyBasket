using ServeyBasket.Contracts.Common;

namespace ServeyBasket.Services.Questions;

public interface IQuestionServices
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
    Task<Result<PaginatedList<QuestionResponse>>> GetAll(int pollId, RequestFilters filters);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvailable(int pollId, string userId);
    Task<Result<QuestionResponse>> Get(int pollId, int questionId);
    Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request);
    Task<Result> ToggleStatusAsync(int pollId, int questionId);
}
