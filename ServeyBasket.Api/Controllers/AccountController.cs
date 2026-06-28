using ServeyBasket.Contracts.Users;
using ServeyBasket.Services.Users;

namespace ServeyBasket.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Profile()
    {
        var user = await _userService.GetUserProfileAsync(User.GetUserId()!);
        return user.IsSuccess 
            ? Ok(user.Value) 
            : user.ToProblem();
    }
    [HttpPut("info")]
    public async Task<IActionResult> Update(UpdateProfileRequest request)
    {
        var user = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
        return user.IsSuccess 
            ? NoContent()
            : user.ToProblem();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
        return user.IsSuccess
            ? NoContent()
            : user.ToProblem();
    }
}
