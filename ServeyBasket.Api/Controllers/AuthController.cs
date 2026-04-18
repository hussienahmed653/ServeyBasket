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

        return authResponse.IsSuccess
            ? Ok(authResponse.Value)
            : Problem(statusCode: StatusCodes.Status400BadRequest,
                        title: authResponse.Error.Code,
                        detail: authResponse.Error.Description);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> GetRefreshToken(RefreshTokenRequest request)
    {
        var refreshTokenResponse = await _authServices.GetRefreshTokenAsync(request);
        return refreshTokenResponse.IsSuccess
            ? Ok(refreshTokenResponse.Value)
            : Problem(statusCode: StatusCodes.Status400BadRequest,
                title: refreshTokenResponse.Error.Code,
                detail: refreshTokenResponse.Error.Description);
    }
}
