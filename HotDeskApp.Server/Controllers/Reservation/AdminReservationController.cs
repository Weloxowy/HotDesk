using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Reservation;

[ApiController]
[Route("admin/")]
public class AdminReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public AdminReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    [Authorize]
    [HttpGet("reservation/all")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
    {
        var reservations = await _reservationService.GetAllReservationsInfo();

        if (reservations == null || !reservations.Any())
        {
            return NotFound();
        }

        return Ok(reservations);
    }
    
    [HttpGet("reservation/{id}")]
    public async Task<ReservationDto> GetResevationById(Guid id)
    {
        return await _reservationService.GetReservationInfo(id);
    }

    [HttpPost("reservation")]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] ReservationEditDto reservation)
    {
        var entity = reservation.ToEntity();
        var userId = await _reservationService.CreateNewReservation(entity);
        return Ok(userId);
    }

    [HttpPut("reservation")]
    public async Task<ActionResult<Guid>> UpdateReservation([FromBody] Models.Reservation.Entities.Reservation reservation)
    {
        await _reservationService.UpdateReservation(reservation);
        return Ok();
    }

    [HttpDelete("reservation/{id}")]
    public async Task<ActionResult<Guid>> DeleteReservation(Guid id)
    {
        await _reservationService.DeleteReservation(id);
        return Ok();
    }
}