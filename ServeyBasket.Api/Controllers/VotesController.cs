

namespace ServeyBasket.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize]
public class VotesController(IQuestionServices questionServices, IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    public async Task<IActionResult> Start(int pollId)
    {
        var userId = User.GetUserId();
        var result = await _questionServices.GetAvailable(pollId, userId!);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request)
    {
        var result = await _voteServices.AddAsync(pollId, User.GetUserId()!, request);
        
        return result.IsSuccess
            ? Created()
            : result.ToProblem();
    }
}
