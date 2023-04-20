using RADTest.Domain.Entities;

namespace RADTest.Domain.Validators;

internal sealed class CannotBeLessThanOneHundred : ITransactionValidator
{
    public string ErrorMessage => "The rest of account balance cannot be less than 100";

    public bool Validate(Account account, double transactionAmount)
    {
        return account.Balance - transactionAmount >= 100;
    }
}
