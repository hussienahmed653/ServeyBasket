using ServeyBasket.Contracts.Questions;

namespace ServeyBasket.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request)
    {
        var result = await _questionServices.AddAsync(pollId, request);

        return result.IsSuccess ? 
            CreatedAtAction(nameof(Get), new { pollId = pollId, result.Value!.Id }, result.Value)
            : result.ToProblem();

    }
}
