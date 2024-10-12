using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Reservation;

[ApiController]
[Route("reservation/")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
    {
        var reservations = await _reservationService.GetAllReservationsInfo();

        if (reservations == null || !reservations.Any())
        {
            return NotFound();
        }

        return Ok(reservations);
    }

//GetReservationsByDeskId
//GetReservationsByLocationId
//GetReservationsByUserId
//GetActiveReservations

    [HttpGet("{id}")]
    public async Task<ReservationDto> GetResevationById(Guid id)
    {
        return await _reservationService.GetReservationInfo(id);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] Models.Reservation.Entities.Reservation reservation)
    {
        var userId = await _reservationService.CreateNewReservation(reservation);
        return Ok(userId);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateReservation([FromBody] Models.Reservation.Entities.Reservation reservation)
    {
        await _reservationService.UpdateReservation(reservation);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Guid>> DeleteReservation(Guid id)
    {
        await _reservationService.DeleteReservation(id);
        return Ok();
    }
}