using System.ComponentModel.DataAnnotations;
using Identity.API.Domains.Users;

namespace Identity.API.Domains.Tokens;

public class RefreshToken
{
    private RefreshToken() { }

    [Key]
    [MaxLength(255)]
    public string TokenId { get; private set; }

    [MaxLength(255)]
    public string Token { get; private set; }

    public DateTime ExpiredAt { get; private set; }

    [MaxLength(450)]
    public string UserId { get; init; } = null!;

    public User User { get; init; } = null!;

    public static RefreshToken Create(string token, DateTime expiredAt)
    {
        var tokenId = Guid.NewGuid().ToString();
        return new RefreshToken
        {
            TokenId = tokenId,
            Token = token,
            ExpiredAt = expiredAt,
        };
    }

    public void Refresh(string token, DateTime expiredAt)
    {
        Token = token;
        ExpiredAt = expiredAt;
    }
}
