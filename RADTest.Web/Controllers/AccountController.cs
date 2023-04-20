using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Responses;
using RADTest.Models;
using System.Collections.ObjectModel;

namespace RADTest.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAccountDomain accountDomain;

    public AccountController(IMapper mapper, IAccountDomain accountDomain)
    {
        this.mapper = mapper;
        this.accountDomain = accountDomain;
    }

    [HttpPost("CreateAccount")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<AccountDto>> CreateAccount()
    {
        var response = await accountDomain.CreateAccountAsync(default);

        return Created(string.Empty, mapper.Map<AccountDto>(response.Model));
    }

    [HttpDelete("DeleteAccount")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAccount([FromQuery] Guid accountId)
    {
        var response = await accountDomain.DeleteAccountAsync(accountId, default);

        return response.Status switch
        {
            ResponseStatus.NoContent => NoContent(),
            ResponseStatus.NotFound => NotFound(response.ErrorMessage),
            _ => throw new InvalidOperationException("Unexpectable result")
        };
    }

    [HttpPut("DepositAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AccountDto>> DepositAccount(Guid accountId, double amount)
    {
        var response = await accountDomain.DepositMoneyAsync(accountId, amount, default);

        return response.Status switch
        {
            ResponseStatus.Success => Ok(mapper.Map<AccountDto>(response.Model)),
            ResponseStatus.Conflict => Conflict(response.ErrorMessage),
            _ => throw new InvalidOperationException("Unexpectable result")
        };
    }

    [HttpPut("WithdrawAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AccountDto>> WithdrawAccount(Guid accountId, double amount)
    {
        var response = await accountDomain.WithdrawMoneyAsync(accountId, amount, default);

        return response.Status switch
        {
            ResponseStatus.Success => Ok(mapper.Map<AccountDto>(response.Model)),
            ResponseStatus.Conflict => Conflict(response.ErrorMessage),
            _ => throw new InvalidOperationException("Unexpectable result")
        };
    }

    [HttpGet("GetAccounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<AccountDto>>> GetAccounts()
    {
        var response = await accountDomain.GetAccountsAsync(default);

        return Ok(mapper.Map<ReadOnlyCollection<AccountDto>>(response.Model));
    }


}
