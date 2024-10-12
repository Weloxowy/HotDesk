using HotDeskApp.Server.Models.Location.Dtos;
using HotDeskApp.Server.Models.Location.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Location;

[ApiController]
[Route("location/")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetAllLocations()
    {
        var locations = await _locationService.GetAllLocationsInfo();
        if (locations == null || !locations.Any())
        {
            return NotFound();
        }

        return Ok(locations);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<LocationDto> GetLocationById(Guid id)
    {
        return await _locationService.GetLocationInfo(id);
    }
}