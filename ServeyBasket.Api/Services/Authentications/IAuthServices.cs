using ServeyBasket.Contracts.Auth;

namespace ServeyBasket.Services.Authentications;

public interface IAuthServices
{
    Task<AuthResponse?> GetTokenAsync(AuthRequest request);
}
