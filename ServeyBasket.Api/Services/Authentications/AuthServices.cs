using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using ServeyBasket.Abstractions.Const;
using ServeyBasket.Helpers;
using RegisterRequest = ServeyBasket.Contracts.Auth.RegisterRequest;

namespace ServeyBasket.Services.Authentications;

public class AuthServices(
    UserManager<ApplicationUser> userManager,
    ServeyBasketDbContext context,
    SignInManager<ApplicationUser> signInManager,
    ILogger<AuthServices> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor,
    IJwtProvider jwtProvider) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ServeyBasketDbContext _context = context;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<AuthServices> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly DateTime _refreshTokenValidityDate = DateTime.UtcNow.AddDays(7);

    public async Task<Result<AuthResponse>> GetTokenAsync(AuthRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user) 
            return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (result.Succeeded)
        {
            var (roles, permissions) = await GetUserRolesAndPermissions(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles, permissions);
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

        return Result.Failuer<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
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
        var (roles, permissions) = await GetUserRolesAndPermissions(user);
        var newToken = _jwtProvider.GenerateToken(user, roles, permissions);
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

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failuer(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failuer(UserErrors.InvalidToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failuer(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        var emailIsAlreadyExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        if (emailIsAlreadyExists)
            return Result.Failuer(UserErrors.DuplicatedEmail);
        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);
        if(result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirm Email Code: {code}", code);

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if(await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failuer(UserErrors.InvalidCode);

        if(user.EmailConfirmed)
            return Result.Failuer(UserErrors.EmailAlreadyConfirmed);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failuer(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

    public async Task<Result> ResendConfirmEmailAsync(ResendConfirmEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failuer(UserErrors.EmailAlreadyConfirmed);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirm Email Code: {code}", code);

        await SendConfirmationEmail(user, code);
        return Result.Success();
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if(!user.EmailConfirmed)
            return Result.Failuer(UserErrors.EmailNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Forget Password Code: {code}", code);

        await SendResetPasswordEmail(user, code);
        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failuer(UserErrors.InvalidCode);

        IdentityResult result;


        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origing = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation", new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origing}/api/auth/confirm-email?userId={user.Id}&code={code}" }
            });

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Confirm your email", emailBody));

        await Task.CompletedTask;
    }
    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origing = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword", new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origing}/api/auth/forget-password?email={user.Email}&code={code}" }
            });

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Confirm your email", emailBody));

        await Task.CompletedTask;
    }

    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await (from r in _context.Roles
                                 join rp in _context.RoleClaims
                                 on r.Id equals rp.RoleId
                                 where roles.Contains(r.Name!)
                                 select rp.ClaimValue)
                                .ToListAsync();

        return (roles, permissions);
    }
}
