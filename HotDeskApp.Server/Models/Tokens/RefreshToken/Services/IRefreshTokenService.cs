namespace HotDeskApp.Server.Models.Tokens.RefreshToken.Services;

public interface IRefreshTokenService
{
    public Task<Entities.RefreshToken> GenerateRefreshToken(Guid userId);
    public Task<Entities.RefreshToken> GetRefreshToken(string token);
    public Task RevokeRefreshToken(string token);
}