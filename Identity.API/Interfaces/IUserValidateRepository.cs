using Shared.Shared;

namespace Identity.API.Interfaces;

public interface IUserValidateRepository
{
    Task<Result<bool>> IsEmailUniqueAsync(string email);
    Task<Result<bool>> IsUsernameUniqueAsync(string username);
}
