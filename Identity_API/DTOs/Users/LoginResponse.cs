namespace Identity.API.DTOs.Users;

public record LoginResponse(string RefreshToken, string AccessToken);
