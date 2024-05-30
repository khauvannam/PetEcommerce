namespace Identity.API.Domain.Users;

internal record LoginResponseDto(string RefreshToken, string AccessToken);
