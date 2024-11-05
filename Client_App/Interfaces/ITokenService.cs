using Client_App.DTOs.Identities.Responses;
using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface ITokenService
{
    public Task<Result<LoginResponse>> Refresh(string accessToken);
    public Task<Result> Revoke(int userId);
}
