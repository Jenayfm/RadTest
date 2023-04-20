using Moq;
using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Factories;
using RADTest.Domain.Responses;
using Xunit;

namespace RADTest.Domain.Test.Factories;

public class WithdrawTransactionFactoryTests
{
    private readonly Mock<ITransactionDomain> mockTransactionDomain;
    private readonly WithdrawTransactionFactory WithdrawTransactionFactory;

    public WithdrawTransactionFactoryTests()
    {
        mockTransactionDomain = new Mock<ITransactionDomain>();
        WithdrawTransactionFactory = new WithdrawTransactionFactory(mockTransactionDomain.Object);
    }

    [Fact]
    public async Task When_WithdrawMoney_Expect_Ok()
    {
        var initialBalance = 1000;
        var account = new Account
        {
            Balance = initialBalance
        };
        var withdraw = 100.23;

        mockTransactionDomain
            .Setup(x => x.CreateTransactionAsync(It.IsAny<Account>(), It.Is<TransactionType>(t => t == TransactionType.Withdraw), It.Is<double>(d => d == -withdraw), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Withdraw, withdraw)));

        var response = await WithdrawTransactionFactory.Create(account, withdraw, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Success, response.Status);
        var transaction = Assert.IsType<Transaction>(response.Model);
        Assert.NotNull(transaction);
        Assert.Equal(initialBalance - withdraw, account.Balance);
    }

    [Fact]
    public async Task When_WithdrawMoreThanNinetyPercent_Expect_Conflict()
    {
        var account = new Account
        {
            Balance = 1000
        };
        var withdraw = 900.01;

        mockTransactionDomain
            .Setup(x => x.CreateTransactionAsync(It.IsAny<Account>(), It.Is<TransactionType>(t => t == TransactionType.Withdraw), It.Is<double>(d => d == -withdraw), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Withdraw, withdraw)));

        var response = await WithdrawTransactionFactory.Create(account, withdraw, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Conflict, response.Status);
    }

    [Fact]
    public async Task When_WithdraWithLessThanOneHundredAsRest_Expect_Conflict()
    {
        var account = new Account
        {
            Balance = 100
        };
        var withdraw = 0.01;

        mockTransactionDomain
            .Setup(x => x.CreateTransactionAsync(It.IsAny<Account>(), It.Is<TransactionType>(t => t == TransactionType.Withdraw), It.Is<double>(d => d == -withdraw), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Withdraw, withdraw)));

        var response = await WithdrawTransactionFactory.Create(account, withdraw, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Conflict, response.Status);
    }
}
