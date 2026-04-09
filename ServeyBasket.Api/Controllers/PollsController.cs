using Microsoft.AspNetCore.Authorization;
using ServeyBasket.Contracts.Polls;
using ServeyBasket.Services.Polls;

namespace ServeyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _pollServices = pollServices;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _pollServices.GetAllAsync();
        
        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var poll = await _pollServices.GetAsync(id);
        if (poll is null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Add(PollRequest request)
    {
        var newpoll = await _pollServices.AddAsync(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll.Adapt<PollResponse>());
    }
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(int id, PollRequest request)
    {
        var IsUpdates = await _pollServices.UpdateAsync(id, request.Adapt<Poll>());
        if (!IsUpdates)
            return NotFound();
        return NoContent();
    }
    [HttpPut]
    [Route("{id:int}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id)
    {
        var IsUpdates = await _pollServices.TogglePublishStatusAsync(id);
        if (!IsUpdates)
            return NotFound();
        return NoContent();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var IsDeleted = await _pollServices.DeletedAsync(id);

        if (!IsDeleted)
            return NotFound();
        return NoContent();
    }
}
