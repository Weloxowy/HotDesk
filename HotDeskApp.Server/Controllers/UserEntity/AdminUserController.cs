using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Models.UserEntity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.UserEntity;

/// <summary>
/// Controller for managing user entities by administrators.
/// Provides endpoints for retrieving, creating, updating, and deleting users.
/// </summary>
[ApiController]
[Route("admin/")]
public class AdminUserController : ControllerBase
{
    private readonly IUserEntityService _userEntityService;
    private readonly TokenHelper _tokenHelper;

    /// <inheritdoc />
    public AdminUserController(IUserEntityService userEntityService, TokenHelper tokenHelper)
    {
        _userEntityService = userEntityService;
        _tokenHelper = tokenHelper;
    }
    
    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <returns>A list of users. Returns HTTP 404 Not Found if no users exist.</returns>
    [Authorize]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserEntityDto>>> GetAllUsers()
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        var users = await _userEntityService.GetAllUsersInfo();

        if (users == null || !users.Any())
        {
            return NotFound();
        }

        return Ok(users);
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The requested user. Returns HTTP 404 Not Found if the user does not exist.</returns>
    [Authorize]
    [HttpGet("users/{id}")]
    public async Task<ActionResult<UserEntityDto>> GetUserById(Guid id)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        return await _userEntityService.GetUserInfo(id);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userEntity">The user entity to create.</param>
    /// <returns>The ID of the newly created user.</returns>
    [Authorize]
    [HttpPost("users")]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] Models.UserEntity.Entities.UserEntity userEntity)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        
        var newUser = await _userEntityService.CreateNewUser(userEntity);
        return Ok(newUser);
    }

    /// <summary>
    /// Updates an existing user by their ID.
    /// </summary>
    /// <param name="userEntity">The updated user entity.</param>
    /// <returns>HTTP 200 OK on success.</returns>
    [Authorize]
    [HttpPut("users/{id}")]
    public async Task<ActionResult<Guid>> UpdateUser([FromBody] Models.UserEntity.Entities.UserEntity userEntity)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        
        await _userEntityService.UpdateUser(userEntity);
        return Ok();
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>HTTP 200 OK on successful deletion.</returns>
    [Authorize]
    [HttpDelete("users/{id}")]
    public async Task<ActionResult<Guid>> DeleteUser(Guid id)
    {
        var userId = await _tokenHelper.VerifyAdmin(HttpContext, TokenHelper.TypeOfReturn.id);
        if (userId == null)
        {
            return Unauthorized("You're not an admin");
        }
        
        await _userEntityService.DeleteUser(id);
        return Ok();
    }
}