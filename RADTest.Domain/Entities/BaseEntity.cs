namespace RADTest.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }

    protected DateTime InsertedDate { get; private set; }

    protected DateTime UpdatedDate { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        InsertedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }
}
