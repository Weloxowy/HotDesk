namespace HotDeskApp.Server.Models.Desk.Dtos;

public class DeskDto
{
    public Guid Id { get; set; }
    public virtual Guid LocationId { get; set; }
    public virtual string LocationName { get; set; }
    public virtual string Name { get; set;}
    public virtual string Description { get; set; }
    public virtual bool IsMaintnance { get; set;}

    public DeskDto(Guid id, Guid locationId, string locationName, string name, string description, bool isMaintnance)
    {
        Id = id;
        LocationId = locationId;
        LocationName = locationName;
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
            desk.Location.Id,
            desk.Location.Name,
            desk.Name,
            desk.Description,
            desk.IsMaintnance
        );
    }
    
}