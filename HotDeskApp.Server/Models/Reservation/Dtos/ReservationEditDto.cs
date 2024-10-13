namespace HotDeskApp.Server.Models.Reservation.Dtos;

public class ReservationEditDto
{
        public ReservationEditDto(Guid userId, Guid deskId, DateTime startDate, DateTime endDate)
        {
            UserId = userId;
            DeskId = deskId;
            StartDate = startDate;
            EndDate = endDate;
        }
        
        public Guid UserId { get; }
        public virtual Guid DeskId { get; }
        public virtual DateTime StartDate { get; }
        public virtual DateTime EndDate { get; }
}

public static class ReservationEditDtoMapping
{
    public static Entities.Reservation ToEntity(this ReservationEditDto dto)
    {
        return new Entities.Reservation
        {
            User = new UserEntity.Entities.UserEntity(dto.UserId),
            Desk = new Desk.Entities.Desk(dto.DeskId),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }
}