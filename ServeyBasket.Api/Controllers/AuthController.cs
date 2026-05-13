namespace ServeyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    private readonly IAuthServices _authServices = authServices;

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var authResponse = await _authServices.GetTokenAsync(request);

        return authResponse.IsSuccess
            ? Ok(authResponse.Value)
            : authResponse.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> GetRefreshToken(RefreshTokenRequest request)
    {
        var refreshTokenResponse = await _authServices.GetRefreshTokenAsync(request);
        return refreshTokenResponse.IsSuccess
            ? Ok(refreshTokenResponse.Value)
            : refreshTokenResponse.ToProblem();
    }
}
