using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Responses;

namespace RADTest.Domain.Domains;

public sealed class TransactionDomain : ITransactionDomain
{
    public async Task<IResponse<Transaction>> CreateTransactionAsync(Account account, TransactionType transactionType, double amount, CancellationToken cancellationToken)
    {
        if (amount == 0)
        {
            return Response<Transaction>.Conflict("Transaction value cannot be equal to zero");
        }

        var transaction = new Transaction(transactionType, amount);
        await Task.Run(() => account.Transactions.Add(transaction), cancellationToken); //immitation of the real Repository

        return Response<Transaction>.Success(transaction);
    }
}
