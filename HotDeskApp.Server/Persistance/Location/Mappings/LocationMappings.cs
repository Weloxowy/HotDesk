using FluentNHibernate.Mapping;

namespace HotDeskApp.Server.Persistance.Location.Mappings;

public class LocationMappings : ClassMap<Models.Location.Entities.Location>
{
    public LocationMappings()
    {
        Table("Location");
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Description);
        Map(x => x.CoverImgPath);
    }
}