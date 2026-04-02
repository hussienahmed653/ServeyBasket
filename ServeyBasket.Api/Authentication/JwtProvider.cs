
namespace ServeyBasket.Authentication;

public class JwtProvider : IJwtProvider
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims =
        [
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.GivenName, user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName, user.LastName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tPMfjVPI2x0U5oRlfdPXhPqIEhhaTWmV"));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var expiresIn = 30; // Token expires in 30 minutes

        var token = new JwtSecurityToken(
            issuer: "ServeyBasket",
            audience: "ServeyBasket users",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: 30 * 60);
    }
}
