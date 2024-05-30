namespace Identity.API.Domain.Tokens;

public record TokenResponseDto(string RefreshToken, string AccessToken);
