using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Responses;

namespace RADTest.Domain.Domains.Interfaces;

public interface ITransactionDomain
{
    Task<IResponse<Transaction>> CreateTransactionAsync(Account account, TransactionType transactionType, double amount, CancellationToken cancellationToken);
}
