using Client_App.DTOs.Identities.Requests;
using Client_App.DTOs.Identities.Responses;
using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface IIdentityService
{
    Task<Result> Register(RegisterFormModel registerModel);
    Task<Result<LoginResponse>> Login(LoginFormModel loginModel);
}
