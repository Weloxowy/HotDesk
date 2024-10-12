using HotDeskApp.Server.Infrastructure;

namespace HotDeskApp.Server.Models.Location.Entities;

public class Location : Entity
{
    public Location()
    {
    }

    public Location(Guid id) : base(id)
    {
    }

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string CoverImgPath { get; set; }
}