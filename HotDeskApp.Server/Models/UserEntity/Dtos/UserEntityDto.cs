using HotDeskApp.Server.Models.UserEntity.Entities;
using HotDeskApp.Server.Models.UserEntity.Enums;

public class UserEntityDto
{
    public Guid Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Email { get; }
    public DateTime LastTimeLogged { get; }
    public UserRole UserRole { get; }

    public UserEntityDto(Guid id, string name, string surname, string email, DateTime lastTimeLogged, UserRole userRole)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        LastTimeLogged = lastTimeLogged;
        UserRole = userRole;
    }
}

public static class UserEntityDtoMapping
{
    public static UserEntityDto ToDto(this UserEntity userEntity)
    {
        return new UserEntityDto(
            userEntity.Id,
            userEntity.Name,
            userEntity.Surname,
            userEntity.Email,
            userEntity.LastTimeLogged,
            userEntity.UserRole
        );
    }
}