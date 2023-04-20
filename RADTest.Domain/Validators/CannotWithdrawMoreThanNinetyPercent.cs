using RADTest.Domain.Entities;

namespace RADTest.Domain.Validators;

internal sealed class CannotWithdrawMoreThanNinetyPercent : ITransactionValidator
{
    public string ErrorMessage => "Cannot withdraw more than 90% of account balance";

    public bool Validate(Account account, double transactionAmount)
    {
        return transactionAmount <= 0.9 * account.Balance;
    }
}
