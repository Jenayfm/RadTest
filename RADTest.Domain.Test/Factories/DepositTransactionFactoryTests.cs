using Moq;
using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Factories;
using RADTest.Domain.Responses;
using Xunit;

namespace RADTest.Domain.Test.Factories;

public class DepositTransactionFactoryTests
{
    private readonly Mock<ITransactionDomain> mockTransactionDomain;
    private readonly DepositTransactionFactory depositTransactionFactory;

    public DepositTransactionFactoryTests()
    {
        mockTransactionDomain = new Mock<ITransactionDomain>();
        depositTransactionFactory = new DepositTransactionFactory(mockTransactionDomain.Object);
    }

    [Fact]
    public async Task When_DepositMoney_Expect_Ok()
    {
        var account = new Account();
        var deposit = 100.23;

        mockTransactionDomain
            .Setup(x => x.CreateTransactionAsync(It.IsAny<Account>(), It.Is<TransactionType>(t => t == TransactionType.Deposit), It.Is<double>(d => d == deposit), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Deposit, deposit)));

        var response = await depositTransactionFactory.Create(account, deposit, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Success, response.Status);
        var transaction = Assert.IsType<Transaction>(response.Model);
        Assert.NotNull(transaction);
        Assert.Equal(deposit, account.Balance);
    }

    [Fact]
    public async Task When_DepositMoreThanTenThousands_Expect_Conflict()
    {
        var account = new Account();
        var deposit = 10000.01;

        mockTransactionDomain
            .Setup(x => x.CreateTransactionAsync(It.IsAny<Account>(), It.Is<TransactionType>(t => t == TransactionType.Deposit), It.Is<double>(d => d == deposit), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Deposit, deposit)));

        var response = await depositTransactionFactory.Create(account, deposit, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Conflict, response.Status);
    }

}
