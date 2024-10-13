namespace HotDeskApp.Server.Models.Desk.Dtos;

public class DeskEditDto
{
    public Guid Id { get; set; }
    public virtual Guid LocationId { get; set; }
    public virtual string Name { get; set;}
    public virtual string Description { get; set; }
    public virtual bool IsMaintnance { get; set;}

    public DeskEditDto(Guid id, Guid locationId, string name, string description, bool isMaintnance)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        IsMaintnance = isMaintnance;
    }
}

public static class DeskEditDtoMapping
{
    public static Entities.Desk ToEntity(this DeskEditDto dto)
    {
        return new Entities.Desk()
        {
            Id = dto.Id,
            Description = dto.Description,
            IsMaintnance = dto.IsMaintnance,
            Location = new Location.Entities.Location(dto.LocationId),
            Name = dto.Name
        };
    }
}