using RADTest.Domain.Entities;
using RADTest.Domain.Responses;


namespace RADTest.Domain.Domains.Interfaces;

public interface IAccountDomain
{
    Task<IResponse<Account>> CreateAccountAsync(CancellationToken cancellationToken);

    Task<IResponse<Account>> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken);

    Task<IResponse<Account>> DepositMoneyAsync(Guid accountId, double amount, CancellationToken cancellationToken);

    Task<IResponse<Account>> WithdrawMoneyAsync(Guid accountId, double amount, CancellationToken cancellationToken);

    Task<IResponse<IReadOnlyCollection<Account>>> GetAccountsAsync(CancellationToken cancellationToken);
}
