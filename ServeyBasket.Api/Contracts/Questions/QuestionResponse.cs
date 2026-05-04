using ServeyBasket.Contracts.Answers;

namespace ServeyBasket.Contracts.Questions;

public record QuestionResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
);