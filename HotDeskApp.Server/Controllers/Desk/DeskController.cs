using HotDeskApp.Server.Models.Desk.Dtos;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Desk;

[ApiController]
[Route("desk/")]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;
    private readonly IReservationService _reservationService;

    public DeskController(IDeskService deskService, IReservationService reservationService)
    {
        _deskService = deskService;
        _reservationService = reservationService;
    }

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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<DeskDto> GetDeskById(Guid id)
    {
        return await _deskService.GetDeskInfo(id);
    }
    
    //if null - actualDate - wykorzystac klase z reservation: najpierw sprawdzamy czy biurko jest wyłączone z uzytkowania
    //potem czy jest zarezerwowane na daną datę
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
    
    //lista wszystkich dostepnych biurek danego dnia
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
            if (!desk.IsMaintnance && !reservations.Any(res => res.StartDate <= reservationDate && res.EndDate >= reservationDate))
            {
                freeDesks.Add(desk);
            }
        }
        return freeDesks;
    }
    
    //zmiana dostępnosci biurka - w uzyciu lub serwis
    [Authorize]
    [HttpPut("change-availability/{deskId}")]
    public async Task ChangeAvailability(Guid deskId, bool isDisabled)
    {
        var desk = await _deskService.GetDeskInfo(deskId);
        if (desk == null)
        {
            throw new ArgumentException("Desk not found");
        }

        var newDesk = new Models.Desk.Entities.Desk()
        {
            Description = desk.Description,
            IsMaintnance = !desk.IsMaintnance,
            Id = deskId,
            LocationId = desk.LocationId,
            Name = desk.Name
        };
        await _deskService.UpdateDesk(newDesk);
    }
    
    
    //dni kiedy biurko jest dostepne a kiedy nie - true/false
    [Authorize]
    [HttpGet("desk-availability/{deskId}")]
    public async Task<IList<(DateTime, bool)>> DeskAvailabilityByMonth(Guid deskId, int month, int year)
    {

        var desk = await _deskService.GetDeskInfo(deskId);
        var reservations = await _reservationService.GetReservationsByDeskId(deskId);
        
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var availabilityList = new List<(DateTime, bool)>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var currentDate = new DateTime(year, month, day);

            bool isFree = !desk.IsMaintnance && !reservations.Any(res => res.StartDate <= currentDate && res.EndDate >= currentDate);
            availabilityList.Add((currentDate, isFree));
        }
        return availabilityList;
    }

    
}