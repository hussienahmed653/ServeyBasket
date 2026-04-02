namespace ServeyBasket.Authentication;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
}
