using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(polls);
    }
    [HttpGet]
    [Route("{id:int}")]
    public IActionResult Get(int id)
    {
        var poll = _pollServices.Get(id);
        if (poll is null)
            return NotFound();
        return Ok(poll);
    }
    [HttpPost]
    public IActionResult Add(Poll request)
    {
        var newpoll = _pollServices.Add(request);
        return CreatedAtAction(nameof(Get), new { id = newpoll.Id }, newpoll);
    }
    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update(int id, Poll request)
    {
        var IsUpdates = _pollServices.Update(id, request);
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
