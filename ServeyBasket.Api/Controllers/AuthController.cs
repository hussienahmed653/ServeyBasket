using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServeyBasket.Contracts.Auth;
using ServeyBasket.Contracts.RefreshToken;
using System.ComponentModel.DataAnnotations;

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
    [HttpPost("refresh")]
    public async Task<IActionResult> GetRefreshToken(RefreshTokenRequest request)
    {
        var refreshTokenResponse = await _authServices.GetRefreshTokenAsync(request);
        return refreshTokenResponse is not null
            ? Ok(refreshTokenResponse)
            : BadRequest("Invalid Token/RefreshToken");
    }
}
