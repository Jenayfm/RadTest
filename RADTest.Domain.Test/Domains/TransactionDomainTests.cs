using RADTest.Domain.Domains;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Responses;
using Xunit;

namespace RADTest.Domain.Test.Domains;

public class TransactionDomainTests
{
    private readonly TransactionDomain transactionDomain;

    public TransactionDomainTests()
    {
        transactionDomain = new TransactionDomain();
    }

    [Fact]
    public async Task When_CreateNewTransaction_Expect_Ok()
    {
        var account = new Account();
        var transactionType = TransactionType.Withdraw;
        var amount = -123.45;

        var result = await transactionDomain.CreateTransactionAsync(account, transactionType, amount, default);

        Assert.NotNull(result);
        Assert.Equal(ResponseStatus.Success, result.Status);
        
        var resultTransaction = Assert.IsType<Transaction>(result.Model);
        Assert.Equal(transactionType.ToString(), resultTransaction.Type);
        Assert.Equal(amount, resultTransaction.Amount);
    }

    [Fact]
    public async Task When_CreateNewZeroTransaction_Expect_Conflict()
    {
        var account = new Account();
        var transactionType = TransactionType.Withdraw;
        var amount = 0;

        var result = await transactionDomain.CreateTransactionAsync(account, transactionType, amount, default);

        Assert.NotNull(result);
        Assert.Equal(ResponseStatus.Conflict, result.Status);
    }
}