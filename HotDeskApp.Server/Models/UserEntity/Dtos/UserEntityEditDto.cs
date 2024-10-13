using HotDeskApp.Server.Models.UserEntity.Enums;

namespace HotDeskApp.Server.Models.UserEntity.Dtos;

public class UserEntityEditDto
{
    public virtual string Name { get; set; }
    public virtual string Surname { get; set; }
    public virtual string Email { get; set; }
    public virtual string PasswordHash { get; set; }
    public virtual DateTime LastTimeLogged { get; set; }
    public virtual UserRole UserRole { get; set; }
}

