using System.Security.Claims;

namespace HotDeskApp.Server.Models.Tokens.JwtToken.Services;

public interface IJwtTokenService
{
    public string GenerateToken(UserEntity.Entities.UserEntity userEntity, string jti);
    public ClaimsPrincipal ValidateToken(string token); 
    public string? GetEmailFromRequestCookie(HttpContext httpContext);
    public string? GetIdFromRequestCookie(HttpContext httpContext);
}