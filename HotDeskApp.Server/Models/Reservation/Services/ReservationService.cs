using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Repositories;

namespace HotDeskApp.Server.Models.Reservation.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationDto?> GetReservationInfo(Guid reservationId)
    {
        var reservation = await _reservationRepository.Get(reservationId);
        return reservation != null ? reservation.ToDto() : null;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsInfo()
    {
        var list = await _reservationRepository.GetAll();
        var newList = new List<ReservationDto>();
        foreach (var reservation in list) newList.Add(reservation.ToDto());
        return newList;
    }

    public async Task<Guid> CreateNewReservation(Entities.Reservation reservation)
    {
        return await _reservationRepository.Save(reservation);
    }

    public async Task UpdateReservation(Entities.Reservation reservation)
    {
        await _reservationRepository.Update(reservation);
    }

    public async Task DeleteReservation(Guid reservationId)
    {
        await _reservationRepository.Delete(reservationId);
    }
    
    public async Task<IEnumerable<ReservationDto>> GetReservationsByDeskId(Guid deskId)
    {
        var reservation = await _reservationRepository.GetAll();
        return reservation.Where(res => res.DeskId == deskId)
            .Select(res => res.ToDto())
            .ToList();
    }
    
    public async Task<IEnumerable<ReservationDto>> GetReservationsByUserId(Guid userId)
    {
        var reservation = await _reservationRepository.GetAll();
        return reservation.Where(res => res.UserId == userId)
            .Select(res => res.ToDto())
            .ToList();
    }
    
    public async Task<IEnumerable<ReservationDto>> GetActiveReservations()
    {
        var reservation = await _reservationRepository.GetAll();
        return reservation.Where(res => res.StartDate <= DateTime.Today && res.EndDate >= DateTime.Today)
            .Select(res => res.ToDto())
            .ToList();
    }
    
    public async Task<IEnumerable<ReservationDto>> GetReservationsByDate(DateTime date)
    {
        var reservation = await _reservationRepository.GetAll();
        return reservation.Where(res => res.StartDate <= date && res.EndDate >= date)
            .Select(res => res.ToDto())
            .ToList();
    }
    
//GetReservationsByLocationId??

}