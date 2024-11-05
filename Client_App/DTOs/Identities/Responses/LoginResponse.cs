namespace Client_App.DTOs.Identities.Responses;

public class LoginResponse(string accessToken, string tokenType = "Bearer")
{
    public string AccessToken { get; set; } = accessToken;
    public string TokenType { get; set; } = tokenType;
}
