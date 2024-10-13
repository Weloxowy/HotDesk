using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Reservation.Dtos;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Criterion;

namespace HotDeskApp.Server.Controllers.Reservation;

[ApiController]
[Route("reservation/")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IDeskService _deskService;
    private readonly TokenHelper _tokenHelper;

    public ReservationController(IReservationService reservationService, TokenHelper tokenHelper, IDeskService deskService)
    {
        _reservationService = reservationService;
        _tokenHelper = tokenHelper;
        _deskService = deskService;
    }

    //tutaj wersja że tylko dla danego usera
    
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

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] ReservationEditDto reservation)
    {
        var entity = reservation.ToEntity();
        var userId = await _reservationService.CreateNewReservation(entity);
        return Ok(userId);
    }

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