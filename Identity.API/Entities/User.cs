using Microsoft.AspNetCore.Identity;

namespace Identity.API.Entities;

public class User : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public RefreshToken? RefreshToken { get; private set; }

    public void AddToken(RefreshToken refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public void RevokeToken()
    {
        RefreshToken = null;
    }
}
