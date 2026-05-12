namespace ServeyBasket.Contracts.Votes;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);
