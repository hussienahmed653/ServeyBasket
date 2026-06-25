namespace ServeyBasket.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] int pollId)
    {
        var result = await _questionServices.GetAll(pollId);
        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }
    [HttpGet("{questionId}")]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int questionId)
    {
        var result = await _questionServices.Get(pollId, questionId);
        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request)
    {
        var result = await _questionServices.AddAsync(pollId, request);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(Get), new { pollId, questionId = result.Value!.Id }, result.Value)
            : result.ToProblem();

    }
    [HttpPut("{questionId}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int questionId, [FromBody] QuestionRequest request)
    {
        var IsUpdates = await _questionServices.UpdateAsync(pollId, questionId, request);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }
    [HttpPut("{questionId}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int questionId)
    {
        var IsUpdates = await _questionServices.ToggleStatusAsync(pollId, questionId);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }
}
