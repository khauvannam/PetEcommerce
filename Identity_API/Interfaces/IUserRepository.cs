using BaseDomain.Results;
using Identity.API.Domains.Users;
using Identity.API.Features.Users;

namespace Identity.API.Interfaces;

internal interface IUserRepository
{
    Task<Result> Register(Register.Command command);
    Task<Result<LoginResponse>> Login(Login.Command command);
    Task<Result<string>> ForgotPassword(ForgotPassword.Command command);
    Task<Result> ResetPassword(ResetPassword.Command command);
}
