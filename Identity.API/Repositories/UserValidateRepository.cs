using Identity.API.Databases;
using Identity.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Shared;

namespace Identity.API.Repositories;

public class UserValidateRepository(UserDbContext dbContext) : IUserValidateRepository
{
    public async Task<Result<bool>> IsEmailUniqueAsync(string email)
    {
        var result = !await dbContext.Users.AnyAsync(u => u.Email == email);
        return Result.Success(result);
    }

    public async Task<Result<bool>> IsUsernameUniqueAsync(string username)
    {
        var result = !await dbContext.Users.AnyAsync(u => u.Email == username);
        return Result.Success(result);
    }
}
