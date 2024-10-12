using HotDeskApp.Server.Models.Tokens.RefreshToken.Repositories;

namespace HotDeskApp.Server.Models.Tokens.RefreshToken.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken>
        GenerateRefreshToken(Guid userId)
    {
        var refreshToken = new HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            IsRevoked = false
        };
        return await _refreshTokenRepository.GenerateRefreshToken(refreshToken);
    }

    public async Task<HotDeskApp.Server.Models.Tokens.RefreshToken.Entities.RefreshToken> GetRefreshToken(string token)
    {
        return await _refreshTokenRepository.GetRefreshToken(token);
    }

    public async Task RevokeRefreshToken(string token)
    {
        await _refreshTokenRepository.RevokeRefreshToken(token);
    }
}