using RADTest.Domain.Entities;

namespace RADTest.Domain.Validators;

internal sealed class CannotDepositMoreThanTenThousands : ITransactionValidator
{
    public string ErrorMessage => "Deposit must be less than 10k";

    public bool Validate(Account account, double transactionAmount)
    {
        return transactionAmount <= 10000;

    }
}
