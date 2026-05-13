namespace ServeyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _pollServices = pollServices;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _pollServices.GetAllAsync());
    }
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        return Ok(await _pollServices.GetCurrentAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var poll = await _pollServices.GetAsync(id);
        return poll.IsSuccess
            ? Ok(poll.Value)
            : poll.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody]PollRequest request)
    {
        var result = await _pollServices.AddAsync(request);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value)
            : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request)
    {
        var IsUpdates = await _pollServices.UpdateAsync(id, request);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id)
    {
        var IsUpdates = await _pollServices.TogglePublishStatusAsync(id);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var IsDeleted = await _pollServices.DeletedAsync(id);

        return IsDeleted.IsSuccess
            ? NoContent()
            : IsDeleted.ToProblem();
    }
}
