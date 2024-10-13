using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotDeskApp.Server.Persistance.Reservation.Mappings;

public class ReservationMapping : ClassMap<Models.Reservation.Entities.Reservation>
{
    public ReservationMapping()
    {
        Table("Reservation");
        Id(x => x.Id);
        References(x => x.User) 
            .Column("UserId")
            .Not.Nullable(); 
        References(x => x.Desk) 
            .Column("DeskId")
            .Not.Nullable(); 
        Map(x => x.StartDate);
        Map(x => x.EndDate);
    }
}