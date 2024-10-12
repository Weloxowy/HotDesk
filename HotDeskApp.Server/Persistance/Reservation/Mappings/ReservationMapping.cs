using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotDeskApp.Server.Persistance.Reservation.Mappings;

public class ReservationMapping : ClassMap<Models.Reservation.Entities.Reservation>
{
    public ReservationMapping()
    {
        Table("Reservation");
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.DeskId);
        Map(x => x.StartDate);
        Map(x => x.EndDate);
    }
}