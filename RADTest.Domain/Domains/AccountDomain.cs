using RADTest.Domain.Context;
using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Factories;
using RADTest.Domain.Global;
using RADTest.Domain.Responses;

namespace RADTest.Domain.Domains;

public sealed class AccountDomain : IAccountDomain
{
    private readonly IContext context;
    private readonly IDepositTransactionFactory depositTransactionFactory;
    private readonly IWithdrawTransactionFactory withdrawTransactionFactory;

    public AccountDomain(IContext context, IDepositTransactionFactory depositTransactionFactory, IWithdrawTransactionFactory withdrawTransactionFactory)
    {
        this.context = context;
        this.depositTransactionFactory = depositTransactionFactory;
        this.withdrawTransactionFactory = withdrawTransactionFactory; 
    }

    public async Task<IResponse<Account>> CreateAccountAsync(CancellationToken cancellationToken)
    {
        var accountObject = new Account();
        await Task.Run(() => context.Accounts.Add(accountObject), cancellationToken); //immitation of the real Repository
        var account = await Task.Run(() => context.Accounts.SingleOrDefault(x => x.Name == accountObject.Name), cancellationToken);

        await depositTransactionFactory.Create(account!, GlobalConfiguration.MinimunBalanceState, cancellationToken);        

        return Response<Account>.Created(account);
    }

    public async Task<IResponse<Account>> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await Task.Run(() => context.Accounts.SingleOrDefault(x => x.Id == accountId), cancellationToken);

        if (account == null)
        {
            return Response<Account>.NotFound("Account does not exist");
        }

        await withdrawTransactionFactory.Create(account, account.Balance, cancellationToken);
        await Task.Run(() => context.Accounts.Remove(account), cancellationToken);
        return Response<Account>.NoContent();
    }

    //TODO: double logic, move to single private function
    public async Task<IResponse<Account>> DepositMoneyAsync(Guid accountId, double amount, CancellationToken cancellationToken)
    {
        var account = await Task.Run(() => context.Accounts.SingleOrDefault(x => x.Id == accountId), cancellationToken);

        if (account == null)
        {
            return Response<Account>.NotFound("Account does not exist");
        }

        var transactionResponse = await depositTransactionFactory.Create(account, amount, cancellationToken);

        if (transactionResponse.Status == ResponseStatus.Success)
        {
            return Response<Account>.Success(account);
        }
        else 
        {
            return Response<Account>.Conflict(transactionResponse.ErrorMessage);
        }
    }

    //TODO: double logic, move to single private function
    public async Task<IResponse<Account>> WithdrawMoneyAsync(Guid accountId, double amount, CancellationToken cancellationToken)
    {
        var account = await Task.Run(() => context.Accounts.SingleOrDefault(x => x.Id == accountId), cancellationToken);

        if (account == null)
        {
            return Response<Account>.NotFound("Account does not exist");
        }

        var transactionResponse = await withdrawTransactionFactory.Create(account, amount, cancellationToken);

        if (transactionResponse.Status == ResponseStatus.Success)
        {
            return Response<Account>.Success(account);
        }
        else
        {
            return Response<Account>.Conflict(transactionResponse.ErrorMessage);
        }
    }

    //Test uncovered!
    public async Task<IResponse<IReadOnlyCollection<Account>>> GetAccountsAsync(CancellationToken cancellationToken)
    {
        var account = context.Accounts == null
            ? new List<Account>().AsReadOnly()
            : await Task.Run(() => context.Accounts.AsReadOnly());

        return Response<IReadOnlyCollection<Account>>.Success(account!);
    }
}
