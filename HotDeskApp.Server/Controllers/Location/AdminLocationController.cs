using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Location.Dtos;
using HotDeskApp.Server.Models.Location.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Location;

/// <summary>
/// Controller for managing locations in the system.
/// Provides endpoints for creating, updating, and deleting locations.
/// </summary>
[ApiController]
[Route("admin/")]
public class AdminLocationController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly TokenHelper _tokenHelper;
    private readonly IDeskService _deskService;

    /// <inheritdoc />
    public AdminLocationController(ILocationService locationService, TokenHelper tokenHelper, IDeskService deskService)
    {
        _locationService = locationService;
        _tokenHelper = tokenHelper;
        _deskService = deskService;
    }

    /// <summary>
    /// Creates a new location.
    /// </summary>
    /// <param name="location">The location to create.</param>
    /// <returns>The ID of the newly created location.</returns>
    [Authorize]
    [HttpPost("location")]
    public async Task<ActionResult<Guid>> CreateLocation([FromBody] Models.Location.Entities.Location location)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }

        var locationId = await _locationService.CreateNewLocation(location);
        return Ok(locationId);
    }

    /// <summary>
    /// Updates an existing location.
    /// </summary>
    /// <param name="location">The location data to update.</param>
    /// <returns>HTTP 200 OK if the update was successful.</returns>
    [Authorize]
    [HttpPut("location")]
    public async Task<ActionResult<Guid>> UpdateLocation([FromBody] Models.Location.Entities.Location location)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        await _locationService.UpdateLocation(location);
        return Ok();
    }

    /// <summary>
    /// Deletes a location by its ID.
    /// </summary>
    /// <param name="id">The ID of the location to delete.</param>
    /// <returns>HTTP 200 OK if the deletion was successful, otherwise an error message.</returns>
    [Authorize]
    [HttpDelete("location/{id}")]
    public async Task<ActionResult<Guid>> DeleteLocation(Guid id)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }

        var isLocationEmpty = await _deskService.GetAllDesksInfoByLocation(id);
        if (isLocationEmpty.Count() > 0)
        {
            return Unauthorized("Location is not empty. Delete all desks from this location first.");
        }

        await _locationService.DeleteLocation(id);
        return Ok();
    }
}