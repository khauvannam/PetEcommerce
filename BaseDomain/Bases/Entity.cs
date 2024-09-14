using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDomain.Bases;

[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Field
)]
public abstract class Entity : Attribute
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
