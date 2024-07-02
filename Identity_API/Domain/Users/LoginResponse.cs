namespace Identity.API.Domain.Users;

internal record LoginResponse(string RefreshToken, string AccessToken);
