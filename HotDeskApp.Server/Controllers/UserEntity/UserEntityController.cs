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

namespace HotDeskApp.Server.Controllers.UserEntity;

[ApiController]
[Route("user/")]
public class UserEntityController: ControllerBase
{
        private readonly IUserEntityService _userEntityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IBlacklistTokenService _blacklistTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly TokenHelper _tokenHelper;

        public UserEntityController(IUserEntityService userEntityService, IJwtTokenService jwtTokenService,
            IBlacklistTokenService blacklistTokenService, IRefreshTokenService refreshTokenService, TokenHelper tokenHelper)
        {
            _userEntityService = userEntityService;
            _jwtTokenService = jwtTokenService;
            _blacklistTokenService = blacklistTokenService;
            _refreshTokenService = refreshTokenService;
            _tokenHelper = tokenHelper;
        }
        
        /*
         * upgrade/downgrade user
         */
        
        
    /// <summary>
    ///     Authenticates the user using standard username-password credentials.
    /// </summary>
    /// <remarks>
    ///     This endpoint verifies the user credentials and provides authentication tokens upon successful validation.
    /// </remarks>
    /// <param name="data">User login data including email and password.</param>
    /// <returns>Authentication tokens upon successful authentication.</returns>
    /// <response code="200">Returns the authentication tokens.</response>
    /// <response code="400">Invalid request or incorrect credentials.</response>
    /// <response code="404">User with the provided email does not exist.</response>
    /// <response code="500">Internal server error occurred.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserByPassword([FromBody] UserLoginDto data)
    {
        var user = await _userEntityService.GetUserInfo(data.Email);
        if (user.Equals(null)) return NotFound("User profile not found");
        var verification = await _userEntityService.VerifyUser(data);
        if (verification == null) return Unauthorized("Credentials are invalid");
        try
        {
            var tokens = await _tokenHelper.GenerateTokens(verification);
            CookieHelper.SetJwtCookie(Response, tokens.JwtToken);
            CookieHelper.SetRefreshTokenCookie(Response, tokens.RefreshToken);

            return Ok("Login successfully");
        }
        catch
        {
            return StatusCode(500, "There was a problem while proceeding a validation. Please try again later");
        }
    }

    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <remarks>
    ///     This endpoint creates a new user based on the provided registration data and provides authentication tokens upon
    ///     successful registration.
    /// </remarks>
    /// <param name="data">User registration data.</param>
    /// <returns>Authentication tokens upon successful registration.</returns>
    /// <response code="200">Returns the authentication tokens.</response>
    /// <response code="400">Invalid request or user data.</response>
    /// <response code="500">Internal server error occurred.</response>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<Models.UserEntity.Entities.UserEntity>> RegisterUser([FromBody] UserRegisterDto data)
    {
        var user = await _userEntityService.GetUserInfo(data.Email);
        if (user != null) return BadRequest("Email address is already used");
        //TODO: back later
        try
        {
            await _userEntityService.ValidateRegisterData(data);
        }
        catch(ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
        
        var newUser = await _userEntityService.RegisterUser(data);
        if (newUser == null) return StatusCode(500, "There was a problem while creating new user.");

        var tokens = await _tokenHelper.GenerateTokens(newUser);
        CookieHelper.SetJwtCookie(Response, tokens.JwtToken);
        CookieHelper.SetRefreshTokenCookie(Response, tokens.RefreshToken);
        return Ok("Registered a new account and login successfully");
    }

    /// <summary>
    ///     Logged out an user.
    /// </summary>
    /// <remarks>
    ///     This endpoint performs a logout operation.
    /// </remarks>
    /// <returns>Authentication tokens upon successful registration.</returns>
    /// <response code="200">Logout performed successfully.</response>
    /// <response code="500">Internal server error occurred.</response>
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        try
        {
            var mail = _jwtTokenService.GetEmailFromRequestCookie(HttpContext);
            if (mail.IsEmpty() || mail == null)
            {
                return Unauthorized("User profile not found");
            }
            var user = await _userEntityService.GetUserInfo(mail);
            if (user == null) return NotFound("User profile not found");

            var authorizationHeader = HttpContext.Request.Cookies["Authorization"];
            if (authorizationHeader == null || authorizationHeader.StartsWith("Bearer "))
                Response.Cookies.Delete("Authorization");

            var refreshTokenHeader = HttpContext.Request.Cookies["RefreshToken"];
            if (refreshTokenHeader != null)
            {
                var refreshToken = refreshTokenHeader.Trim();
                _blacklistTokenService.AddToBlacklist(refreshToken, user.Id, DateTime.UtcNow.AddHours(2));
                await _refreshTokenService.RevokeRefreshToken(refreshToken);
                Response.Cookies.Delete("RefreshToken");
            }

            return Ok("User logged out successfully.");
        }
        catch
        {
            return NotFound("Cookies were not found");
        }
        
    }
        
        
        //BASIC
        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserEntityDto>>> GetAllUsers()
        {
            var users = await _userEntityService.GetAllUsersInfo();

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserEntityDto> GetUserById(Guid id)
        {
            return await _userEntityService.GetUserInfo(id);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody]Models.UserEntity.Entities.UserEntity userEntity)
        {
            var userId = await _userEntityService.CreateNewUser(userEntity);
            return Ok(userId);
        }
        
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Guid>> UpdateUser([FromBody]Models.UserEntity.Entities.UserEntity userEntity)
        {
            await _userEntityService.UpdateUser(userEntity);
            return Ok();
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> UpdateUser(Guid id)
        {
            await _userEntityService.DeleteUser(id);
            return Ok();
        }

}