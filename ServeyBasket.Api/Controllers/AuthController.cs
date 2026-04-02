using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServeyBasket.Contracts.Auth;

namespace ServeyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;

    [HttpPost]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var authResponse = await _authServices.GetTokenAsync(request);

        return authResponse is not null
            ? Ok(authResponse)
            : BadRequest("Invalid Email/Password");
    }
}
