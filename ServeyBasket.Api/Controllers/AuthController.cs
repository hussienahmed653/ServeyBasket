using ServeyBasket.Contracts.Users;

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
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authServices.RevokeRefreshTokenAsync(request.Token!, request.RefreshToken!);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerResponse = await _authServices.RegisterAsync(request);
        return registerResponse.IsSuccess
            ? Ok()
            : registerResponse.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        var registerResponse = await _authServices.ConfirmEmailAsync(request);
        return registerResponse.IsSuccess
            ? Ok()
            : registerResponse.ToProblem();
    }
    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmEmail(ResendConfirmEmailRequest request)
    {
        var registerResponse = await _authServices.ResendConfirmEmailAsync(request);
        return registerResponse.IsSuccess
            ? Ok()
            : registerResponse.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
    {
        var registerResponse = await _authServices.SendResetPasswordCodeAsync(request.Email);
        return registerResponse.IsSuccess
            ? Ok()
            : registerResponse.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var registerResponse = await _authServices.ResetPasswordAsync(request);
        return registerResponse.IsSuccess
            ? Ok()
            : registerResponse.ToProblem();
    }

}
