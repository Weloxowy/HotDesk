using FluentNHibernate.Mapping;

namespace HotDeskApp.Server.Persistance.Desk.Mappings;

public class DeskMappings : ClassMap<Models.Desk.Entities.Desk>
{
    DeskMappings()
    {
        Table("Desk");
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Description);
        Map(x => x.IsMaintnance);
        References(x => x.Location) 
            .Column("LocationId")
            .Not.Nullable(); 
    }
}