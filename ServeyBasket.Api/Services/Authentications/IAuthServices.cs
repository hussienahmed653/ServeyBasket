namespace ServeyBasket.Services.Authentications;

public interface IAuthServices
{
    Task<Result<AuthResponse>> GetTokenAsync(AuthRequest request);
    Task<Result<RefreshTokenRequest>> GetRefreshTokenAsync(RefreshTokenRequest request);
}
