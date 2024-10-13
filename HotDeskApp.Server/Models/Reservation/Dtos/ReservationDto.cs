namespace HotDeskApp.Server.Models.Reservation.Dtos;

public class ReservationDto
{
    public ReservationDto(Guid id, Guid userId, string name, string surname, Guid deskId, string deskName, Guid locationId, string locationName, DateTime startDate, DateTime endDate)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Surname = surname;
        DeskId = deskId;
        DeskName = deskName;
        LocationId = locationId;
        LocationName = locationName;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public virtual string Name { get; }
    public virtual string Surname { get; }
    public virtual Guid DeskId { get; }
    public virtual string DeskName { get; }
    public virtual Guid LocationId { get; }
    public virtual string LocationName { get; }
    public virtual DateTime StartDate { get; }
    public virtual DateTime EndDate { get; }
}

public static class ReservationDtoMapping
{
    public static ReservationDto ToDto(this Entities.Reservation reservation)
    {
        return new ReservationDto(
            reservation.Id,
            reservation.User.Id,
            reservation.User.Name,
            reservation.User.Surname,
            reservation.Desk.Id,
            reservation.Desk.Name,
            reservation.Desk.Location.Id,
            reservation.Desk.Location.Name,
            reservation.StartDate,
            reservation.EndDate
        );
    }
}