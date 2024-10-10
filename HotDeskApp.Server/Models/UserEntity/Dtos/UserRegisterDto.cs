namespace HotDeskApp.Server.Models.UserEntity.Dtos;

public class UserRegisterDto
{
    public virtual string Name { get; set; }
    public virtual string Surname { get; set; }
    public virtual string Email { get; set; }
    public virtual string PasswordHash { get; set; }
}