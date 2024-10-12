using FluentNHibernate.Conventions;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Models.UserEntity.Entities;
using HotDeskApp.Server.Models.UserEntity.Enums;
using HotDeskApp.Server.Models.UserEntity.Services;

namespace HotDeskApp.Server.Infrastructure;

public class TokenHelper
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenService _tokenEntityService;
    private readonly IBlacklistTokenService _blacklistTokenService;
    private readonly IUserEntityService _userEntityService;

    public TokenHelper(IRefreshTokenService refreshTokenService, IJwtTokenService tokenEntityService,
        IBlacklistTokenService blacklistTokenService, IUserEntityService userEntityService)
    {
        _refreshTokenService = refreshTokenService;
        _tokenEntityService = tokenEntityService;
        _blacklistTokenService = blacklistTokenService;
        _userEntityService = userEntityService;
    }

    public TokenHelper(IBlacklistTokenService blacklistTokenService, IUserEntityService userEntityService) : base()
    {
        _blacklistTokenService = blacklistTokenService;
        _userEntityService = userEntityService;
    }

    public async Task<(string JwtToken, string RefreshToken)> GenerateTokens(UserEntity user)
    {
        var refresh = await _refreshTokenService.GenerateRefreshToken(user.Id);
        var jwt = _tokenEntityService.GenerateToken(user, refresh.Id.ToString());
        return (jwt, refresh.Token);
    }

    public async Task RevokeTokens(string jwtToken, string refreshToken, Guid userId)
    {
        await _refreshTokenService.RevokeRefreshToken(refreshToken);
        _blacklistTokenService.AddToBlacklist(jwtToken, userId, DateTime.Now.AddHours(3));
        _blacklistTokenService.AddToBlacklist(refreshToken, userId, DateTime.Now.AddHours(3));
    }

    public enum TypeOfReturn
    {
        email,
        id
    }

    public async Task<string?> VerifyUser(HttpContext httpContext, TypeOfReturn type)
    {
        if (type == TypeOfReturn.email)
        {
            var mail = _tokenEntityService.GetEmailFromRequestCookie(httpContext);
            if (mail.IsEmpty() || mail == null)
            {
                return null;
            }

            return mail;
        }
        else
        {
            var id = _tokenEntityService.GetIdFromRequestCookie(httpContext);
            if (id.IsEmpty() || id == null)
            {
                return null;
            }

            return id;
        }
    }

    public async Task<string?> VerifyAdmin(HttpContext httpContext, TypeOfReturn type)
    {
        string cookieValue;
        if (type == TypeOfReturn.email)
        {
            var mail = _tokenEntityService.GetEmailFromRequestCookie(httpContext);
            if (mail.IsEmpty() || mail == null)
            {
                return null;
            }

            cookieValue = mail;
        }
        else
        {
            var id = _tokenEntityService.GetIdFromRequestCookie(httpContext);
            if (id.IsEmpty() || id == null)
            {
                return null;
            }

            cookieValue = id;
        }

        var user = type == TypeOfReturn.email
            ? await _userEntityService.GetUserInfo(cookieValue)
            : await _userEntityService.GetUserInfo(Guid.Parse(cookieValue));
        if (user.UserRole != UserRole.Admin)
        {
            return null;
        }

        return cookieValue;
    }
}