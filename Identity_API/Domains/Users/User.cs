using System.ComponentModel.DataAnnotations;
using Base.Bases;
using Identity.API.Domains.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domains.Users;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();

    public RefreshToken? RefreshToken { get; private set; }
    public Address Address { get; private set; } = null!;

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
    private Address() { }

    [MaxLength(100)]
    public string? Street { get; private init; }

    [MaxLength(100)]
    public string? City { get; private init; }

    [MaxLength(100)]
    public string? ZipCode { get; private init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street!;
        yield return City!;
        yield return ZipCode!;
    }

    public static Address Create(string street, string city, string zipCode)
    {
        return new Address
        {
            Street = street,
            City = city,
            ZipCode = zipCode,
        };
    }
}
