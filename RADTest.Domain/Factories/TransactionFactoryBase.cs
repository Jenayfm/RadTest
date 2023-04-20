using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Entities;
using RADTest.Domain.Validators;

namespace RADTest.Domain.Factories;

public abstract class TransactionFactoryBase
{
    protected IList<ITransactionValidator> Validators { get; private set;} = new List<ITransactionValidator>();
    protected string ErrorMessages { get; private set; } = string.Empty;

    
    protected ITransactionDomain transactionDomain;


    protected TransactionFactoryBase(ITransactionDomain transactionDomain)
    {
        this.transactionDomain = transactionDomain;
    }

    protected bool ValidateTransaction(Account account, double amount)
    {
        var result = new List<string>();

        foreach (var validator in Validators)
        {
            if (!validator.Validate(account, amount))
            {
                result.Add(validator.ErrorMessage);
            }
        }

        ErrorMessages = string.Join(", ", result);

        return string.IsNullOrEmpty(ErrorMessages);
    }
}
