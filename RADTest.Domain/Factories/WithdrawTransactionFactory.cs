using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Responses;
using RADTest.Domain.Validators;

namespace RADTest.Domain.Factories;

public class WithdrawTransactionFactory : TransactionFactoryBase, IWithdrawTransactionFactory
{
    public WithdrawTransactionFactory(ITransactionDomain transactionDomain) : base(transactionDomain) 
    {
        Validators.Add(new CannotBeLessThanOneHundred());
        Validators.Add(new CannotWithdrawMoreThanNinetyPercent());
    }

    public async Task<IResponse<Transaction>> Create(Account account, double amount, CancellationToken cancellationToken)
    {
        if (ValidateTransaction(account, amount))
        {
            var transactionResponse = await transactionDomain.CreateTransactionAsync(account, TransactionType.Withdraw, -amount, cancellationToken);
            account.Balance += transactionResponse.Model!.Amount;
            account.Transactions.Add(transactionResponse.Model!);

            return transactionResponse;
        }

        return Response<Transaction>.Conflict(ErrorMessages);
    }
}
