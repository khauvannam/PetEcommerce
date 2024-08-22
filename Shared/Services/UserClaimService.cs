using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Services;

public class UserClaimService(IHttpContextAccessor accessor)
{
    public Claim? GetUserClaimType(string claimType)
    {
        return accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public IEnumerable<Claim> GetUserClaims()
    {
        return accessor.HttpContext?.User.Claims
            ?? throw new InvalidOperationException("No user claims found.");
    }
}
