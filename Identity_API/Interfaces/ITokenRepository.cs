using Base.Results;
using Identity.API.Domains.Tokens;
using Identity.API.DTOs.Tokens;
using Identity.API.Features.Tokens;

namespace Identity.API.Interfaces;

public interface ITokenRepository
{
    public Task<Result<TokenResponse>> Refresh(Refresh.Command command);
    public Task<Result> Revoke(Revoke.Command command);
}
