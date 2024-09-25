using BasedDomain.Bases;

namespace Order.API.Domains.Payments;

public class Payment : ValueObject
{
    private Payment() { }

    public Guid PaymentId { get; private set; }
    public decimal Amount { get; private init; }
    public PaymentMethod Method { get; private init; }

    public static Payment ProcessPayment(decimal amount, PaymentMethod method)
    {
        var payment = new Payment
        {
            Amount = amount,
            Method = method switch
            {
                PaymentMethod.Bank => PaymentMethod.Bank,
                PaymentMethod.CreditCard => PaymentMethod.CreditCard,
                PaymentMethod.Momo => PaymentMethod.Momo,
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
            },
        };
        return payment;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Method;
    }
}

public enum PaymentMethod
{
    Bank,
    Momo,
    CreditCard,
}
