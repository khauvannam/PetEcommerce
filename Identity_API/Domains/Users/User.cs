using BaseDomain.Bases;
using Identity.API.Domains.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domains.Users;

public class User : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public RefreshToken? RefreshToken { get; private set; }
    public Address Address { get; private set; } = Address.Create();

    public void AddToken(RefreshToken refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public void RevokeToken()
    {
        RefreshToken = null;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }
}

public class Address : ValueObject
{
    private Address()
    {
    }

    public string Street { get; private init; } = null!;
    public string City { get; private init; } = null!;
    public string ZipCode { get; private init; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }

    public static Address Create(string street = "", string city = "", string zipCode = "") =>
        new()
        {
            Street = street,
            City = city,
            ZipCode = zipCode
        };
}