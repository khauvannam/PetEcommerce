using Client_App.Abstraction;
using Client_App.DTOs.Identities.Requests;
using Client_App.DTOs.Identities.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;

namespace Client_App.Services.APIs;

public class IdentityService(
    IHttpClientFactory factory,
    string baseUrl = nameof(IdentityService),
    string endpoint = "api/User"
) : BaseApiService(factory, baseUrl, endpoint), IIdentityService
{
    public async Task<Result> Register(RegisterFormModel registerModel)
    {
        var content = SerializeItemToJson(registerModel);
        var result = await Client.PostAsync($"{Endpoint}/Register", content);
        return await HandleResponse(result);
    }

    public async Task<Result<LoginResponse>> Login(LoginFormModel loginModel)
    {
        var content = SerializeItemToJson(loginModel);
        var result = await Client.PostAsync($"{Endpoint}/Login", content);
        return await HandleResponse<LoginResponse>(result);
    }
}
