using Microsoft.AspNetCore.Authorization;

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
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var poll = await _pollServices.GetAsync(id);
        return poll.IsSuccess
            ? Ok(poll.Value)
            : poll.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> Add(PollRequest request)
    {
        var result = await _pollServices.AddAsync(request);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value!.Id }, result.Value)
            : result.ToProblem();
    }
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(int id, PollRequest request)
    {
        var IsUpdates = await _pollServices.UpdateAsync(id, request);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }
    [HttpPut]
    [Route("{id:int}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id)
    {
        var IsUpdates = await _pollServices.TogglePublishStatusAsync(id);
        return IsUpdates.IsSuccess
            ? NoContent()
            : IsUpdates.ToProblem();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var IsDeleted = await _pollServices.DeletedAsync(id);

        return IsDeleted.IsSuccess
            ? NoContent()
            : IsDeleted.ToProblem();
    }
}
