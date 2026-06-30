namespace ServeyBasket.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class ResultsController(IResultServices resultServices) : ControllerBase
{
    private readonly IResultServices _resultServices = resultServices;

    [HttpGet("row-data")]
    public async Task<IActionResult> PollVotes([FromRoute] int pollId)
    {
        var result = await _resultServices.GetPollVotesAsync(pollId);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("votes-per-day")]
    public async Task<IActionResult> VotesPerDay([FromRoute] int pollId)
    {
        var result = await _resultServices.GetVotesPerDayAsync(pollId);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }

    [HttpGet("votes-per-question")]
    public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId)
    {
        var result = await _resultServices.GetVotesPerQuestionAsync(pollId);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }

}
