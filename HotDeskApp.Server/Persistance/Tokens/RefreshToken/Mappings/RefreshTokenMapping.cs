using FluentNHibernate.Mapping;

namespace HotDeskApp.Server.Persistance.Tokens.RefreshToken.Mappings;

public class RefreshTokenMapping : ClassMap<HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken>
{
    public RefreshTokenMapping()
    {
        Table("RefreshToken");
        Id(x => x.Id);
        Map(x => x.Token);
        Map(x => x.Expiration);
        Map(x => x.IsRevoked);
        Map(x => x.UserId);
    }
}