using Identity.API.Entities;
using Identity.API.Features.Tokens;
using Shared.Shared;

namespace Identity.API.Interfaces;

public interface ITokenRepository
{
    public Task<Result<TokenResponseDto>> Refresh(Refresh.Command command);
    public Task<Result> Revoke(Revoke.Command command);
}
