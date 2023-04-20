using RADTest.Domain.Entities;

namespace RADTest.Domain.Context;

public class Context : IContext
{
    public IList<Account> Accounts { get; set; } = new List<Account>();
}
