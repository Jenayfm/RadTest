using RADTest.Domain.Entities;

namespace RADTest.Domain.Validators;

public interface ITransactionValidator
{
    public string ErrorMessage { get; }

    bool Validate(Account account, double transactionAmount);
}
