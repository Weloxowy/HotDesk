namespace HotDeskApp.Server.Models.Location.Dtos;

public class LocationDto
{
    public LocationDto(Guid id, string name, string description, string coverImgPath)
    {
        Id = id;
        Name = name;
        Description = description;
        CoverImgPath = coverImgPath;
    }

    public Guid Id { get; }
    public virtual string Name { get; }
    public virtual string Description { get; }
    public virtual string CoverImgPath { get; }
}

public static class LocationDtoMapping
{
    public static LocationDto ToDto(this Entities.Location location)
    {
        return new LocationDto(
            location.Id,
            location.Name,
            location.Description,
            location.CoverImgPath
        );
    }
}