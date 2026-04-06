using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace ServeyBasket.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public List<RefreshToken> RefreshTokens { get; set; } = [];
}
