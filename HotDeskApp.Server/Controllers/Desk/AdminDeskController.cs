using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Desk;

[ApiController]
[Route("admin/")]
public class AdminDeskController : ControllerBase
{
    private readonly IDeskService _deskService;
    private readonly TokenHelper _tokenHelper;

    public AdminDeskController(IDeskService deskService, TokenHelper tokenHelper)
    {
        _deskService = deskService;
        _tokenHelper = tokenHelper;
    }
    
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
}