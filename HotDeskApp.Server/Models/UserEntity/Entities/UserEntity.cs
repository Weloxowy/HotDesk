using HotDeskApp.Server.Models.UserEntity.Enums;
using HotDeskApp.Server.Infrastructure;

namespace HotDeskApp.Server.Models.UserEntity.Entities;

public class UserEntity : Entity
{
    public UserEntity() : base()
    {
    }

    public UserEntity(Guid id) : base(id)
    {
    }

    public UserEntity(Guid id, string name, string surname, string email, string passwordHash, DateTime lastTimeLogged,
        UserRole userRole) : base(id)
    {
        Name = name;
        Surname = surname;
        Email = email;
        PasswordHash = passwordHash;
        LastTimeLogged = lastTimeLogged;
        UserRole = userRole;
    }

    public virtual string Name { get; set; }
    public virtual string Surname { get; set; }
    public virtual string Email { get; set; }
    public virtual string PasswordHash { get; set; }
    public virtual DateTime LastTimeLogged { get; set; }
    public virtual UserRole UserRole { get; set; }
}