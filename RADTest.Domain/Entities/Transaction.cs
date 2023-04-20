using RADTest.Domain.Entities.Enums;

namespace RADTest.Domain.Entities;

public sealed class Transaction : BaseEntity
{
    public string Type { get; private set; }

    public double Amount { get; private set; }

    public Transaction(TransactionType type, double amount)
    {
        Amount = amount;
        Type = type.ToString();
    }
}
