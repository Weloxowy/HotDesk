using HotDeskApp.Server.Models.Desk.Dtos;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Desk;

/// <summary>
/// Controller for managing desk operations.
/// Provides endpoints to retrieve desk information, check availability, and more.
/// </summary>
[ApiController]
[Route("desk/")]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;
    private readonly IReservationService _reservationService;

    /// <inheritdoc />
    public DeskController(IDeskService deskService, IReservationService reservationService)
    {
        _deskService = deskService;
        _reservationService = reservationService;
    }

    /// <summary>
    /// Retrieves all desks.
    /// </summary>
    /// <returns>A list of DeskDto objects representing all desks.</returns>
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetAllDesks()
    {
        var desks = await _deskService.GetAllDesksInfo();

        if (desks == null || !desks.Any())
        {
            return NotFound();
        }

        return Ok(desks);
    }

    /// <summary>
    /// Retrieves desks filtered by location.
    /// </summary>
    /// <param name="locationId">The ID of the location for which to retrieve desks.</param>
    /// <returns>A list of DeskDto objects representing desks at the specified location.</returns>
    [Authorize]
    [HttpGet("all/location/{locationId}")]
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetDesksByLocation(Guid locationId)
    {
        var desks = await _deskService.GetAllDesksInfoByLocation(locationId);

        if (desks == null || !desks.Any())
        {
            return NotFound();
        }

        return Ok(desks);
    }

    /// <summary>
    /// Retrieves a specific desk by its ID.
    /// </summary>
    /// <param name="id">The ID of the desk to retrieve.</param>
    /// <returns>The DeskDto object representing the specified desk.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<DeskDto> GetDeskById(Guid id)
    {
        return await _deskService.GetDeskInfo(id);
    }
    
    /// <summary>
    /// Checks if a specific desk is busy at a given time.
    /// </summary>
    /// <param name="deskId">The ID of the desk to check.</param>
    /// <param name="timeOfReservation">The time to check for availability (defaults to now).</param>
    /// <returns>True if the desk is busy, otherwise false.</returns>
    [Authorize]
    [HttpGet("availability/{deskId}")]
    public async Task<bool> IsDeskBusy(Guid deskId, DateTime? timeOfReservation = null)
    {
        var reservationDate = timeOfReservation ?? DateTime.Now;
        var desk = await _deskService.GetDeskInfo(deskId);
        if (desk == null || desk.IsMaintnance)
        {
            return true;
        }
        var reservations = await _reservationService.GetReservationsByDeskId(deskId);
        return reservations.Any(res => res.StartDate <= reservationDate && res.EndDate >= reservationDate);
    }
    
    /// <summary>
    /// Retrieves a list of all available desks for a given day.
    /// </summary>
    /// <param name="timeOfReservation">The date to check for available desks (defaults to now).</param>
    /// <returns>A list of DeskDto objects representing all available desks.</returns>
    [Authorize]
    [HttpGet("/all/free")]
    public async Task<IEnumerable<DeskDto>> GetAllFreeDesks(DateTime? timeOfReservation = null)
    {
        var reservationDate = timeOfReservation ?? DateTime.Now;
        var allDesks = await _deskService.GetAllDesksInfo();
        var freeDesks = new List<DeskDto>();
        foreach (var desk in allDesks)
        {
            var reservations = await _reservationService.GetReservationsByDeskId(desk.Id);
            if (!desk.IsMaintnance && !reservations.Any(res => reservationDate >= res.StartDate && reservationDate <= res.EndDate))
            {
                freeDesks.Add(desk);
            }
        }
        return freeDesks;
    }
    
    /// <summary>
    /// Checks the availability of a desk for each day of a specified month.
    /// </summary>
    /// <param name="deskId">The ID of the desk to check availability for.</param>
    /// <param name="month">The month to check (1-12).</param>
    /// <param name="year">The year to check.</param>
    /// <returns>A list of booleans indicating the availability of the desk for each day of the month.</returns>
    [Authorize]
    [HttpGet("desk-availability/{deskId}")]
    public async Task<IList<bool>> DeskAvailabilityByMonth(Guid deskId, int month, int year)
    {

        var desk = await _deskService.GetDeskInfo(deskId);
        var reservations = await _reservationService.GetReservationsByDeskId(deskId);
        
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var availabilityList = new List<bool>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var currentDate = new DateTime(year, month, day);

            bool isFree = !desk.IsMaintnance && !reservations.Any(res => res.StartDate.Day <= currentDate.Day && res.EndDate.Day >= currentDate.Day);
            availabilityList.Add(isFree);
        }
        return  availabilityList;
    }
}