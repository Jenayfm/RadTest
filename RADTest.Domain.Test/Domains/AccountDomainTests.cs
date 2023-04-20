using Moq;
using RADTest.Domain.Context;
using RADTest.Domain.Domains;
using RADTest.Domain.Entities;
using RADTest.Domain.Entities.Enums;
using RADTest.Domain.Factories;
using RADTest.Domain.Global;
using RADTest.Domain.Responses;
using Xunit;

namespace RADTest.Domain.Test.Domains;

public class AccountDomainTests
{
    private readonly Mock<IContext> mockContext;
    private readonly Mock<IDepositTransactionFactory> mockDepositTransactionFactory;
    private readonly Mock<IWithdrawTransactionFactory> mockWithdrawTransactionFactory;
    private readonly AccountDomain accountDomain; 

    public AccountDomainTests()
    {
        mockContext = new Mock<IContext>();
        mockDepositTransactionFactory = new Mock<IDepositTransactionFactory>();
        mockWithdrawTransactionFactory = new Mock<IWithdrawTransactionFactory>();

        accountDomain = new AccountDomain(mockContext.Object, mockDepositTransactionFactory.Object, mockWithdrawTransactionFactory.Object);
    }

    [Fact]
    public async Task When_CreateNewAccountWithUser_Expect_Ok()
    {
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>());

        mockDepositTransactionFactory
            .Setup(x => x.Create(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Deposit, GlobalConfiguration.MinimunBalanceState)));

        var response = await accountDomain.CreateAccountAsync(default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Created, response.Status);

        var account = Assert.IsType<Account>(response.Model);
        Assert.NotNull(account);
    }

    [Fact]
    public async Task When_DeleteAccount_Expect_NoContent()
    {
        var account = new Account();
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>(new[] { account }));

        var response = await accountDomain.DeleteAccountAsync(account.Id, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.NoContent, response.Status);
    }

    [Fact]
    public async Task When_DeleteNotExistingAccount_Expect_NotFound()
    {
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>());

        var response = await accountDomain.DeleteAccountAsync(Guid.NewGuid(), default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.NotFound, response.Status);
    }

    [Fact]
    public async Task When_DepositAccount_Expect_Success()
    {
        var account = new Account();
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>(new[] { account }));

        mockDepositTransactionFactory
            .Setup(x => x.Create(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Deposit, GlobalConfiguration.MinimunBalanceState)));

        var response = await accountDomain.DepositMoneyAsync(account.Id, GlobalConfiguration.MinimunBalanceState, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Success, response.Status);
    }

    [Fact]
    public async Task When_DepositAccount_Expect_Conflict()
    {
        var account = new Account();
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>(new[] { account }));

        mockDepositTransactionFactory
            .Setup(x => x.Create(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Conflict(string.Empty));

        var response = await accountDomain.DepositMoneyAsync(account.Id, GlobalConfiguration.MinimunBalanceState, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Conflict, response.Status);
    }

    [Fact]
    public async Task When_WithdrawAccount_Expect_Success()
    {
        var account = new Account();
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>(new[] { account }));

        mockWithdrawTransactionFactory
            .Setup(x => x.Create(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Success(new Transaction(TransactionType.Deposit, GlobalConfiguration.MinimunBalanceState)));

        var response = await accountDomain.WithdrawMoneyAsync(account.Id, GlobalConfiguration.MinimunBalanceState, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Success, response.Status);
    }

    [Fact]
    public async Task When_WithdrawAccount_Expect_Conflict()
    {
        var account = new Account();
        mockContext
            .Setup(x => x.Accounts)
            .Returns(new List<Account>(new[] { account }));

        mockWithdrawTransactionFactory
            .Setup(x => x.Create(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response<Transaction>.Conflict(string.Empty));

        var response = await accountDomain.WithdrawMoneyAsync(account.Id, GlobalConfiguration.MinimunBalanceState, default);

        Assert.NotNull(response);
        Assert.Equal(ResponseStatus.Conflict, response.Status);
    }
}