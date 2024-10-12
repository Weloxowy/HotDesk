namespace HotDeskApp.Server.Models.Reservation.Dtos;

public class ReservationDto
{
    public ReservationDto(Guid id, Guid userId, Guid deskId, DateTime startDate, DateTime endDate)
    {
        Id = id;
        UserId = userId;
        DeskId = deskId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; }
    public virtual Guid UserId { get; }
    public virtual Guid DeskId { get; }
    public virtual DateTime StartDate { get; }
    public virtual DateTime EndDate { get; }
}

public static class ReservationDtoMapping
{
    public static ReservationDto ToDto(this Entities.Reservation reservation)
    {
        return new ReservationDto(
            reservation.Id,
            reservation.UserId,
            reservation.DeskId,
            reservation.StartDate,
            reservation.EndDate
        );
    }
}