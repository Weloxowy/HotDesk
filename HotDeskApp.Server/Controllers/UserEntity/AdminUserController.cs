using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Models.UserEntity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.UserEntity;

[ApiController]
[Route("admin/")]
public class AdminUserController : ControllerBase
{
    private readonly IUserEntityService _userEntityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IBlacklistTokenService _blacklistTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly TokenHelper _tokenHelper;

    public AdminUserController(IUserEntityService userEntityService, IJwtTokenService jwtTokenService,
        IBlacklistTokenService blacklistTokenService, IRefreshTokenService refreshTokenService, TokenHelper tokenHelper)
    {
        _userEntityService = userEntityService;
        _jwtTokenService = jwtTokenService;
        _blacklistTokenService = blacklistTokenService;
        _refreshTokenService = refreshTokenService;
        _tokenHelper = tokenHelper;
    }

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