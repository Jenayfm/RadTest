using RADTest.Domain.Entities;
using RADTest.Domain.Responses;

namespace RADTest.Domain.Factories;

public interface ITransactionFactory
{
    Task<IResponse<Transaction>> Create(Account account, double amount, CancellationToken cancellationToken);
}
