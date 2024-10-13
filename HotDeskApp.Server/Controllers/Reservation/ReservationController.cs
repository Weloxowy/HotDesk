using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Reservation;

/// <summary>
/// Controller for managing reservations in the application.
/// Provides endpoints for creating, updating, retrieving, and deleting user-specific reservations.
/// </summary>
[ApiController]
[Route("reservation/")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IDeskService _deskService;
    private readonly TokenHelper _tokenHelper;

    /// <inheritdoc />
    public ReservationController(IReservationService reservationService, TokenHelper tokenHelper, IDeskService deskService)
    {
        _reservationService = reservationService;
        _tokenHelper = tokenHelper;
        _deskService = deskService;
    }

    /// <summary>
    /// Retrieves all reservations for the authenticated user.
    /// </summary>
    /// <returns>A list of reservations for the current user. Returns HTTP 404 Not Found if no reservations exist.</returns>
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("User not found");
        }
        var reservations = await _reservationService.GetAllReservationsInfo();
        var reservationDtos = reservations.ToList();
        if (reservations == null || !reservationDtos.Any())
        {
            return NotFound();
        }
        return Ok(reservationDtos.Where(x => x.UserId == Guid.Parse(userId)).ToList());
    }
    
    /// <summary>
    /// Retrieves a reservation by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the reservation to retrieve.</param>
    /// <returns>The requested reservation. Returns HTTP 404 Not Found if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetResevationById(Guid id)
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("User not found");
        }

        var reservation = await _reservationService.GetReservationInfo(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return reservation.UserId == Guid.Parse(userId) ? Ok(reservation) : Unauthorized();
    }

    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <param name="reservation">The reservation details to create.</param>
    /// <returns>The ID of the created reservation.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] ReservationEditDto reservation)
    {
        var entity = reservation.ToEntity();
        var userId = await _reservationService.CreateNewReservation(entity);
        return Ok(userId);
    }
    /// <summary>
    /// Updates an existing reservation for the authenticated user.
    /// </summary>
    /// <param name="reservation">The updated reservation details.</param>
    /// <returns>HTTP 200 OK on success or HTTP 401 Unauthorized if the user does not own the reservation.</returns>
    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateReservation([FromBody] Models.Reservation.Entities.Reservation reservation)
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("User not found");
        }

        var reservationFromDb = await _reservationService.GetReservationInfo(reservation.Id);
        if (reservationFromDb.UserId != Guid.Parse(userId))
        {
            return Unauthorized();
        }
        await _reservationService.UpdateReservation(reservation);
        return Ok();
    }

    /// <summary>
    /// Deletes a reservation by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the reservation to delete.</param>
    /// <returns>HTTP 200 OK on successful deletion or HTTP 401 Unauthorized if the user does not own the reservation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Guid>> DeleteReservation(Guid id)
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("User not found");
        }
        var reservation = await _reservationService.GetReservationInfo(id);
        if (reservation.UserId != Guid.Parse(userId))
        {
            return Unauthorized();
        }

        if ((reservation.StartDate - DateTime.Now).TotalHours  < 24)
        {
            return Unauthorized("You can't cancel your reservation now.");
        }
        await _reservationService.DeleteReservation(id);
        return Ok();
    }
}