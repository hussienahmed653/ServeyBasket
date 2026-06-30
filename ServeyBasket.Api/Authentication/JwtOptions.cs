namespace ServeyBasket.Authentication;

public class JwtOptions
{
    public static string SectionName { get; set; } = "Jwt";
    [Required]
    public string Key { get; init; } = string.Empty;
    [Required]
    public string Issuer { get; init; } = string.Empty;
    [Required]
    public string Audience { get; init; } = string.Empty;
    [Required, Range(1, int.MaxValue)]
    public int ExpireMinutes { get; init; }
}
