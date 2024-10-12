namespace HotDeskApp.Server.Models.Desk.Dtos;

public class DeskDto
{
    public Guid Id { get; set; }
    public virtual Guid LocationId { get; set; }
    public virtual string Name { get; set;}
    public virtual string Description { get; set; }
    public virtual bool IsMaintnance { get; set;}

    public DeskDto(Guid id, Guid locationId, string name, string description, bool isMaintnance)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        IsMaintnance = isMaintnance;
    }
}

public static class DeskDtoMapping
{
    public static DeskDto ToDto(this Entities.Desk desk)
    {
        return new DeskDto(
            desk.Id,
            desk.LocationId,
            desk.Name,
            desk.Description,
            desk.IsMaintnance
        );
    }
}