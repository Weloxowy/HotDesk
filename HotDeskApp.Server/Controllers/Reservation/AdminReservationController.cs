using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Reservation;


/// <summary>
/// Controller for managing reservations in the application.
/// Provides endpoints for creating, updating, retrieving, and deleting reservations.
/// </summary>
[ApiController]
[Route("admin/")]
public class AdminReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    /// <inheritdoc />
    public AdminReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    /// <summary>
    /// Retrieves all reservations.
    /// </summary>
    /// <returns>A list of reservations. Returns HTTP 404 Not Found if no reservations exist.</returns>
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
    
    /// <summary>
    /// Retrieves a reservation by its ID.
    /// </summary>
    /// <param name="id">The ID of the reservation to retrieve.</param>
    /// <returns>The requested reservation. If not found, returns HTTP 404 Not Found.</returns>
    [HttpGet("reservation/{id}")]
    public async Task<ReservationDto> GetResevationById(Guid id)
    {
        return await _reservationService.GetReservationInfo(id);
    }

    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <param name="reservation">The reservation details to create.</param>
    /// <returns>The ID of the created reservation.</returns>
    [HttpPost("reservation")]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] ReservationEditDto reservation)
    {
        var entity = reservation.ToEntity();
        var userId = await _reservationService.CreateNewReservation(entity);
        return Ok(userId);
    }

    /// <summary>
    /// Updates an existing reservation.
    /// </summary>
    /// <param name="reservation">The updated reservation details.</param>
    /// <returns>HTTP 200 OK on success.</returns>
    [HttpPut("reservation")]
    public async Task<ActionResult<Guid>> UpdateReservation([FromBody] Models.Reservation.Entities.Reservation reservation)
    {
        await _reservationService.UpdateReservation(reservation);
        return Ok();
    }

    /// <summary>
    /// Deletes a reservation by its ID.
    /// </summary>
    /// <param name="id">The ID of the reservation to delete.</param>
    /// <returns>HTTP 200 OK on successful deletion.</returns>
    [HttpDelete("reservation/{id}")]
    public async Task<ActionResult<Guid>> DeleteReservation(Guid id)
    {
        await _reservationService.DeleteReservation(id);
        return Ok();
    }
}