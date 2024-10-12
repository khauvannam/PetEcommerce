using Base.Results;

namespace Identity.API.Interfaces;

public interface IUserServiceRepository
{
    Task<Result<bool>> IsEmailUniqueAsync(string email);
    Task<Result<bool>> IsUsernameUniqueAsync(string username);
}
