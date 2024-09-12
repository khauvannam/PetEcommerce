using BaseDomain.Bases;

namespace Order.API.Domains.Payments;

public class Payment : Entity
{
    private Payment() { }

    public string PaymentId
    {
        get => Id;
        private set => Id = value;
    }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string TransactionReference { get; set; } = null!;

    public static Payment Create(decimal amount)
    {
        return new() { Amount = amount };
    }

    public void ProcessPayment(PaymentMethod paymentMethod)
    {
        switch (paymentMethod)
        {
            case PaymentMethod.Bank:
                break;
            case PaymentMethod.Momo:
                break;
            case PaymentMethod.CreditCard:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(paymentMethod), paymentMethod, null);
        }
    }
}

public enum PaymentMethod
{
    Bank,
    Momo,
    CreditCard,
}
