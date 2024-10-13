using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Desk;

/// <summary>
/// Controller for managing desks (desk) for administrators.
/// Allows creating, updating, deleting, and changing the availability of desks.
/// </summary>
[ApiController]
[Route("admin/")]
public class AdminDeskController : ControllerBase
{
    private readonly IDeskService _deskService;
    private readonly TokenHelper _tokenHelper;

    /// <inheritdoc />
    public AdminDeskController(IDeskService deskService, TokenHelper tokenHelper)
    {
        _deskService = deskService;
        _tokenHelper = tokenHelper;
    }
    
    /// <summary>
    /// Creates a new desk.
    /// </summary>
    /// <param name="desk">The desk object to be created.</param>
    /// <returns>The ID of the newly created desk.</returns>
    [Authorize]
    [HttpPost("desk")]
    public async Task<ActionResult<Guid>> CreateDesk([FromBody] Models.Desk.Entities.Desk desk)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        var newDesk = await _deskService.CreateNewDesk(desk);
        return Ok(newDesk);
    }

    /// <summary>
    /// Creates multiple desks.
    /// </summary>
    /// <param name="desk">The desk object to clone.</param>
    /// <param name="numberOfDesks">The number of desks to create.</param>
    /// <returns>The ID of the newly created desks.</returns>
    [Authorize]
    [HttpPost("desks")]
    public async Task<ActionResult<Guid>> CreateManyDesks([FromBody] Models.Desk.Entities.Desk desk, int numberOfDesks)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        var newDesks = await _deskService.CreateNewDesks(desk, numberOfDesks);
        return Ok(newDesks);
    }
    
    /// <summary>
    /// Updates the desk's information.
    /// </summary>
    /// <param name="desk">The desk object with the new data.</param>
    /// <returns>The ID of the updated desk.</returns>
    [Authorize]
    [HttpPut("desk/{id}")]
    public async Task<ActionResult<Guid>> UpdateDesk([FromBody] Models.Desk.Entities.Desk desk)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        
        await _deskService.UpdateDesk(desk);
        return Ok();
    }
    
    /// <summary>
    /// Deletes the desk with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the desk to delete.</param>
    /// <returns>The ID of the deleted desk.</returns>
    [Authorize]
    [HttpDelete("desk/{id}")]
    public async Task<ActionResult<Guid>> DeleteDesk(Guid id)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        
        await _deskService.DeleteDesk(id);
        return Ok();
    }
    
    /// <summary>
    /// Changes the availability of the desk.
    /// </summary>
    /// <param name="deskId">The ID of the desk whose availability is to be changed.</param>
    /// <param name="isDisabled">A value indicating whether the desk should be disabled (true) or enabled (false).</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Authorize]
    [HttpPut("change-availability/{deskId}")]
    public async Task ChangeAvailability(Guid deskId, bool isDisabled)
    {
        var desk = await _deskService.GetDeskEntityInfo(deskId);
        if (desk == null)
        {
            throw new ArgumentException("Desk not found");
        }

        var newDesk = new Models.Desk.Entities.Desk()
        {
            Description = desk.Description,
            IsMaintnance = !desk.IsMaintnance,
            Id = deskId,
            Location = desk.Location,
            Name = desk.Name
        };
        await _deskService.UpdateDesk(newDesk);
    }

}