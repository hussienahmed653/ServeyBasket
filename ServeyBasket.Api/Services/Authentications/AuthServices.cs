using ServeyBasket.Authentication;
using ServeyBasket.Contracts.Auth;
using ServeyBasket.Contracts.RefreshToken;
using System.Security.Cryptography;

namespace ServeyBasket.Services.Authentications;

public class AuthServices(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly DateTime _refreshTokenValidityDate = DateTime.UtcNow.AddDays(7);

    public async Task<AuthResponse?> GetTokenAsync(AuthRequest request)
    {
        //var user = await _userManager.FindByEmailAsync(request.Email);

        //if (user == null) return null;

        //var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        //if (!isValidPassword) return null;
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            FirstName = "John",
            LastName = "Doe"
        };

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();

        //user.RefreshTokens.Add(new RefreshToken
        //{
        //    Token = refreshToken,
        //    ExpiresOn = _refreshTokenValidityDate
        //});
        await _userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, _refreshTokenValidityDate);
    }
    public async Task<RefreshTokenRequest?> GetRefreshTokenAsync(RefreshTokenRequest request)
    {
        var tokenId = _jwtProvider.ValidateToken(request.Token!);
        if (tokenId == null)
            return null;

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "request.Email",
            FirstName = "John",
            LastName = "Doe"
        };

        //var user = await _userManager.FindByIdAsync(tokenId);

        //if (user == null)
        //    return null;

        //var refreshTokenId = user!.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);

        //if (!tokenId.Equals(refreshTokenId))
        //    return null;

        var newToken = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();

        //user.RefreshTokens.Select(x => x.RevokedOn = DateTime.UtcNow);
        //user.RefreshTokens.Add(new RefreshToken
        //{
        //    Token = newRefreshToken,
        //    ExpiresOn = _refreshTokenValidityDate
        //});
        return new RefreshTokenRequest(newToken.token, newRefreshToken);
    }
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
