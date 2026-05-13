namespace ServeyBasket.Contracts.Results;

public record VoteResponse(
    string VoterName,
    DateTime VotedAt,
    IEnumerable<QuestionAnswerResponse> SelectedAnswers
);
