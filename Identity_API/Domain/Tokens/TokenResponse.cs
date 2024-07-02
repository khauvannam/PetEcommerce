namespace Identity.API.Domain.Tokens;

public record TokenResponse(string RefreshToken, string AccessToken);
