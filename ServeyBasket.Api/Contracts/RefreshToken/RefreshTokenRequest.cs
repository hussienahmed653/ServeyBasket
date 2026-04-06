namespace ServeyBasket.Contracts.RefreshToken;

public record RefreshTokenRequest(
    string? Token,
    string? RefreshToken
);
