namespace Identity.API.Domains.Users;

public record LoginResponse(string RefreshToken, string AccessToken);
