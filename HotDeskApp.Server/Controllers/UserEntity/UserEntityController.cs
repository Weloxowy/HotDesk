using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Conventions;
using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Repositories;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Models.UserEntity.Dtos;
using HotDeskApp.Server.Models.UserEntity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HotDeskApp.Server.Controllers.UserEntity;

[ApiController]
[Route("user/")]
public class UserEntityController : ControllerBase
{
    private readonly IUserEntityService _userEntityService;
    private readonly TokenHelper _tokenHelper;

    public UserEntityController(IUserEntityService userEntityService, TokenHelper tokenHelper)
    {
        _userEntityService = userEntityService;
        _tokenHelper = tokenHelper;
    }
    
    /// <summary>
    /// Retrieves a user by their ID from token.
    /// </summary>
    /// <returns>The requested user. Returns HTTP 404 Not Found if the user does not exist.</returns>
    [Authorize]
    [HttpGet("user")]
    public async Task<UserEntityDto> GetUserByToken()
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        return await _userEntityService.GetUserInfo(Guid.Parse(userId));
    }

    /// <summary>
    /// Updates a user by their ID.
    /// </summary>
    /// <param name="userEntity">The entity of the user to update.</param>
    /// <returns>HTTP 200 response. Returns HTTP 404 Not Found if the user does not exist.</returns>
    [Authorize]
    [HttpPut("user")]
    public async Task<ActionResult<Guid>> UpdateUserByToken([FromBody] Models.UserEntity.Entities.UserEntity userEntity)
    {
        await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        await _userEntityService.UpdateUser(userEntity);
        return Ok();
    }

    /// <summary>
    /// Deletes a user by their ID from token.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>HTTP 200 OK on successful deletion.</returns>
    [Authorize]
    [HttpDelete("user")]
    public async Task<ActionResult<Guid>> DeleteUser()
    {
        var userId = await _tokenHelper.VerifyUser(HttpContext, TokenHelper.TypeOfReturn.id);
        await _userEntityService.DeleteUser(Guid.Parse(userId));
        return Ok();
    }
}