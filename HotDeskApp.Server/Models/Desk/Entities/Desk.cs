using HotDeskApp.Server.Infrastructure;

namespace HotDeskApp.Server.Models.Desk.Entities;

public class Desk : Entity
{
    public Desk() : base()
    {
    }

    public Desk(Guid id) : base(id)
    {
    }

    public Desk(Guid id, Guid locationId, string name, string description, bool isMaintnance) : base(id)
    {
        LocationId = locationId;
        Name = name;
        Description = description;
        IsMaintnance = isMaintnance;
    }

    public virtual Guid LocationId { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsMaintnance { get; set; }
}