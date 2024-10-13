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

    public virtual Location.Entities.Location Location { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsMaintnance { get; set; }
}