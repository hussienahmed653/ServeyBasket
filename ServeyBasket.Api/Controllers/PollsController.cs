using Microsoft.AspNetCore.Authorization;

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

        return polls.IsSuccess
            ? Ok(polls.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: polls.Error.Code, detail: polls.Error.Description);
    }
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var poll = await _pollServices.GetAsync(id);
        return poll.IsSuccess
            ? Ok(poll.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: poll.Error.Code, detail: poll.Error.Description);
    }
    [HttpPost]
    public async Task<IActionResult> Add(PollRequest request)
    {
        var newpoll = await _pollServices.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll);
    }
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(int id, PollRequest request)
    {
        var IsUpdates = await _pollServices.UpdateAsync(id, request);
        return IsUpdates.IsSuccess
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: IsUpdates.Error.Code, detail: IsUpdates.Error.Description);
    }
    [HttpPut]
    [Route("{id:int}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id)
    {
        var IsUpdates = await _pollServices.TogglePublishStatusAsync(id);
        return IsUpdates.IsSuccess
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: IsUpdates.Error.Code, detail: IsUpdates.Error.Description);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var IsDeleted = await _pollServices.DeletedAsync(id);

        return IsDeleted.IsSuccess
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: IsDeleted.Error.Code, detail: IsDeleted.Error.Description);
    }
}
