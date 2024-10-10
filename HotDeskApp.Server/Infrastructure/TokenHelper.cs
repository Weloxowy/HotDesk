using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Models.UserEntity.Entities;

namespace HotDeskApp.Server.Infrastructure;

public class TokenHelper
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenService _tokenEntityService;

    public TokenHelper(IRefreshTokenService refreshTokenService, IJwtTokenService tokenEntityService)
    {
        _refreshTokenService = refreshTokenService;
        _tokenEntityService = tokenEntityService;
    }

    public TokenHelper() : base()
    {
       
    }
    public async Task<(string JwtToken, string RefreshToken)> GenerateTokens(UserEntity user)
    {
        var refresh =  await _refreshTokenService.GenerateRefreshToken(user.Id);
        var jwt = _tokenEntityService.GenerateToken(user, refresh.Id.ToString());
        return (jwt, refresh.Token);
    }
}