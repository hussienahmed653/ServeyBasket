using ServeyBasket.Contracts.Auth;
using ServeyBasket.Contracts.RefreshToken;

namespace ServeyBasket.Services.Authentications;

public interface IAuthServices
{
    Task<AuthResponse?> GetTokenAsync(AuthRequest request);
    Task<RefreshTokenRequest?> GetRefreshTokenAsync(RefreshTokenRequest request);
}
