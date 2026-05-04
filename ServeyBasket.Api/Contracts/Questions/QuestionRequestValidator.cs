namespace ServeyBasket.Contracts.Questions;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Length(3,1000);

        RuleFor(x => x.Answers)
            .NotEmpty();

        RuleFor(x => x.Answers)
            .Must(x => x.Count > 1)
            .WithMessage("At least two answers are required.")
            .When(x => x.Answers is not null);

        RuleFor(x => x.Answers)
            .Must(x => x.Distinct().Count() == x.Count())
            .WithMessage("Answers must be unique for the same question.")
            .When(x => x.Answers is not null);
    }
}
