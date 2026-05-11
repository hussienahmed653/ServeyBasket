using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServeyBasket.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize]
public class VotesController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;

    [HttpGet("")]
    public async Task<IActionResult> Start(int pollId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _questionServices.GetAvailable(pollId, userId!);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
