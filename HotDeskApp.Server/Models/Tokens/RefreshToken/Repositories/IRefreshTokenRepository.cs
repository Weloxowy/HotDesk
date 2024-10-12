namespace HotDeskApp.Server.Models.Tokens.RefreshToken.Repositories;

public interface IRefreshTokenRepository
{
    public Task<Entities.RefreshToken> GenerateRefreshToken(Entities.RefreshToken refreshToken);
    public Task<Entities.RefreshToken> GetRefreshToken(string token);
    public Task RevokeRefreshToken(string token);
}