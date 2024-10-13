using HotDeskApp.Server.Models.Reservation.Dtos;

namespace HotDeskApp.Server.Models.Reservation.Services;

public interface IReservationService
{
    public Task<ReservationDto?> GetReservationInfo(Guid deskId);
    public Task<IEnumerable<ReservationDto>> GetAllReservationsInfo();
    public Task<Guid> CreateNewReservation(Entities.Reservation reservation);
    public Task UpdateReservation(Entities.Reservation reservation);
    public Task DeleteReservation(Guid reservationId);
    public Task<IEnumerable<ReservationDto>> GetReservationsByDeskId(Guid deskId);
    public Task<IEnumerable<ReservationDto>> GetReservationsByUserId(Guid userId);
    public Task<IEnumerable<ReservationDto>> GetActiveReservations();
    public Task<IEnumerable<ReservationDto>> GetReservationsByDate(DateTime date);
}