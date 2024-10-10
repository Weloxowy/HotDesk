using FluentNHibernate.Mapping;
using HotDeskApp.Server.Models.UserEntity.Enums;

namespace HotDeskApp.Server.Persistance.UserEntity.Mappings;

public class UserEntityMapping : ClassMap<Models.UserEntity.Entities.UserEntity>
{
    public UserEntityMapping()
    {
        Table("UserEntity");
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Surname);
        Map(x => x.Email);
        Map(x => x.PasswordHash);
        Map(x => x.LastTimeLogged);
        Map(x => x.UserRole).CustomType<UserRole>();
    }
}