namespace ServeyBasket.Abstractions;

public static class ResultExtention
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Can't convert successfull method to problem");

        var problem = Results.Problem(statusCode: result.Error.statusCode);
        var problemDetailes = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
        problemDetailes!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors", new[] {
                    new
                    {
                        result.Error.Code,
                        result.Error.Description
                    }
                }
            }
        };
        return new ObjectResult(problemDetailes);
    }
}
