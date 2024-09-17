namespace BaseDomain.Bases;

public abstract class Entity
{
    protected Guid Id { get; init; } = Guid.NewGuid();

    protected int ClusterId { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var entity = (Entity)obj;

        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
