using Microsoft.AspNetCore.Identity;
using ServeyBasket.Authentication;
using ServeyBasket.Contracts.Auth;

namespace ServeyBasket.Services.Authentications;

public class AuthServices(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if(user == null) return null;

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if(!isValidPassword) return null;

        // Generate token logic here (e.g., using JWT)
        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);

    }
}
