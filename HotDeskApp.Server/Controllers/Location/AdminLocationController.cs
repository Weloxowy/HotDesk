using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Location.Dtos;
using HotDeskApp.Server.Models.Location.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Location;

[ApiController]
[Route("admin/")]
public class AdminLocationController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly TokenHelper _tokenHelper;
    private readonly IDeskService _deskService;

    public AdminLocationController(ILocationService locationService, TokenHelper tokenHelper, IDeskService deskService)
    {
        _locationService = locationService;
        _tokenHelper = tokenHelper;
        _deskService = deskService;
    }

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