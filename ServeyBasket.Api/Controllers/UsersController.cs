namespace ServeyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] CreateUserRequest request)
    {
        var result = await _userService.AddAsync(request);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new {result.Value!.Id}, result.Value)
            : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request)
    {
        var result = await _userService.UpdateAsync(id, request);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _userService.ToggleStatusAsync(id);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpPut("{id}/unlock")]
    public async Task<IActionResult> Unlock([FromRoute] string id)
    {
        var result = await _userService.Unlock(id);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
