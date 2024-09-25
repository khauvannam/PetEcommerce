using BasedDomain.Results;
using Identity.API.Domains.Users;
using Identity.API.Features.Users;

namespace Identity.API.Interfaces;

public interface IUserRepository
{
    Task<Result> Register(User user, string password);
    Task<Result<LoginResponse>> Login(Login.Command command);
    Task<Result> ResetPassword(ResetPassword.Command command);
}
