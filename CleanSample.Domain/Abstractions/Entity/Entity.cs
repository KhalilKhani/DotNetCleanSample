namespace CleanSample.Domain.Abstractions.Entity;

public abstract class Entity<TId> : IEntity
{
    public TId Id { get; protected set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; protected set; }

    // This constructor is needed for EF
    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == null || other.Id == null)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id!.GetHashCode();
    }
}