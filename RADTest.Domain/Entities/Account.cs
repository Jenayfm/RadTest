namespace RADTest.Domain.Entities;

public sealed class Account : BaseEntity
{
    public string Name { get; private set; }

    public double Balance { get; set; }

    public IList<Transaction> Transactions { get; private set; } = new List<Transaction>();

    public Account() : base() 
    {
        Name = Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}