using Microsoft.AspNetCore.Identity;
using Shared.Entities.Bases;

namespace Identity.API.Entities;

public class User : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public RefreshToken? RefreshToken { get; private set; }
    public Address Address { get; private set; } = Address.Create("1", "1", "1");

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
        if (Address.Equals(address))
            return;

        Address = address;
    }
}

public class Address(string street, string city, string zipCode) : ValueObject
{
    public string Street { get; private set; } = street;
    public string City { get; private set; } = city;
    public string ZipCode { get; private set; } = zipCode;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }

    public static Address Create(string street, string city, string zipCode) =>
        new(street, city, zipCode);
}
