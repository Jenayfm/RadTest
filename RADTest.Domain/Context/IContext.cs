using RADTest.Domain.Entities;

namespace RADTest.Domain.Context;

public interface IContext
{
    IList<Account> Accounts { get; set; }
}
