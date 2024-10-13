using HotDeskApp.Server.Models.Location.Dtos;
using HotDeskApp.Server.Models.Location.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Location;

/// <summary>
/// Controller for managing locations within the application.
/// Provides endpoints for retrieving location information.
/// </summary>
[ApiController]
[Route("location/")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    /// <inheritdoc />
    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /// <summary>
    /// Retrieves all locations.
    /// </summary>
    /// <returns>A list of locations. Returns HTTP 404 Not Found if no locations exist.</returns>
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

    /// <summary>
    /// Retrieves a location by its ID.
    /// </summary>
    /// <param name="id">The ID of the location to retrieve.</param>
    /// <returns>The requested location. If the location is not found, returns HTTP 404 Not Found.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<LocationDto> GetLocationById(Guid id)
    {
        return await _locationService.GetLocationInfo(id);
    }
}