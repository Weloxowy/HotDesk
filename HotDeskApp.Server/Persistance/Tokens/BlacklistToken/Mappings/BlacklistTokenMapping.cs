using FluentNHibernate.Mapping;

namespace HotDeskApp.Server.Persistance.Tokens.BlacklistToken.Mappings;

public class BlacklistTokenMapping : ClassMap<Models.Tokens.BlacklistToken.Entities.BlacklistToken>
{
    public BlacklistTokenMapping()
    {
        Table("BlacklistToken");
        Id(x => x.Token);
        Map(x => x.EndOfBlacklisting);
        Map(x => x.UserId);
    }
}