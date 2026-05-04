namespace ServeyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new("Question.NotFound", "No question was found", statusCode: StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuestionContent =
        new("Question.DuplicatedContent", "There is another question with the same content", statusCode: StatusCodes.Status409Conflict);
}
