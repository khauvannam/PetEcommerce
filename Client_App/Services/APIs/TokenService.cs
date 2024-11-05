using Client_App.Abstraction;
using Client_App.DTOs.Identities.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;

namespace Client_App.Services.APIs;

public class TokenService(
    IHttpClientFactory factory,
    string baseUrl = nameof(IdentityService),
    string endpoint = "api/token"
) : BaseApiService(factory, baseUrl, endpoint), ITokenService
{
    public async Task<Result<LoginResponse>> Refresh(string accessToken)
    {
        var content = SerializeItemToJson(accessToken);
        var result = Client.PostAsync($"{Endpoint}/refresh", content).Result;
        return await HandleResponse<LoginResponse>(result);
    }

    public async Task<Result> Revoke(int userId)
    {
        var content = SerializeItemToJson(userId.ToString());
        var result = Client.PostAsync($"{Endpoint}/revoke", content).Result;
        return await HandleResponse(result);
    }
}
