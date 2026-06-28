namespace ServeyBasket.Services.Authentications;

public interface IAuthServices
{
    Task<Result<AuthResponse>> GetTokenAsync(AuthRequest request);
    Task<Result<RefreshTokenRequest>> GetRefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken);
    Task<Result> RegisterAsync(RegisterRequest request);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmEmailAsync(ResendConfirmEmailRequest request);
    Task<Result> SendResetPasswordCodeAsync(string email);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}
