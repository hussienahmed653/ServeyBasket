using Mapster;
using Microsoft.AspNetCore.Mvc;
using ServeyBasket.Contracts.Requests;
using ServeyBasket.Contracts.Respons;
using ServeyBasket.Models;
using ServeyBasket.Services;

namespace ServeyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollServices pollServices) : ControllerBase
{
    private readonly IPollServices _pollServices = pollServices;

    [HttpGet]
    public IActionResult GetAll()
    {
        var polls = _pollServices.GetAll();
        
        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }
    [HttpGet]
    [Route("{id:int}")]
    public IActionResult Get(int id)
    {
        var poll = _pollServices.Get(id);
        if (poll is null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();

        return Ok(response);
    }
    [HttpPost]
    public IActionResult Add(CreatePollRequest request)
    {
        var newpoll = _pollServices.Add(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll);
    }
    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update(int id, CreatePollRequest request)
    {
        var IsUpdates = _pollServices.Update(id, request.Adapt<Poll>());
        if (!IsUpdates)
            return NotFound();
        return NoContent();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete(int id)
    {
        var IsDeleted = _pollServices.Deleted(id);

        if (!IsDeleted)
            return NotFound();
        return NoContent();
    }
}
