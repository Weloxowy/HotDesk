namespace HotDeskApp.Server.Infrastructure;

public abstract class Entity
{
    protected Entity()
    {
    }

    public Entity(Guid id)
    {
        this.Id = id;
    }

    public virtual Guid Id { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Entity other = obj as Entity;
        if (other != null)
        {
            return Id.Equals(other.Id);
        }

        return base.Equals(obj);
    }
}