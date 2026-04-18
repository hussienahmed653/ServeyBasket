namespace ServeyBasket.Services.Authentications;

public class AuthServices(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly DateTime _refreshTokenValidityDate = DateTime.UtcNow.AddDays(7);

    public async Task<Result<AuthResponse>> GetTokenAsync(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) 
            return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = _refreshTokenValidityDate
        });
        await _userManager.UpdateAsync(user);
        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, _refreshTokenValidityDate);
        return Result.Success(response);
    }
    public async Task<Result<RefreshTokenRequest>> GetRefreshTokenAsync(RefreshTokenRequest request)
    {
        var tokenId = _jwtProvider.ValidateToken(request.Token!);

        if (tokenId == null)
            return Result.Failuer<RefreshTokenRequest>(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(tokenId);

        if (user == null)
            return Result.Failuer<RefreshTokenRequest>(UserErrors.InvalidToken);

        var refreshToken = user!.RefreshTokens.SingleOrDefault(x => (x.Token == request.RefreshToken) && (x.RevokedOn is null));

        if (refreshToken is null)
            return Result.Failuer<RefreshTokenRequest>(UserErrors.InvalidRefreshToken);

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newToken = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshTokens.Select(x => x.RevokedOn = DateTime.UtcNow);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = _refreshTokenValidityDate
        });

        await _userManager.UpdateAsync(user);
        return Result.Success(new RefreshTokenRequest(newToken.token, newRefreshToken));
    }
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
