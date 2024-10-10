namespace HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;

public interface IBlacklistTokenService
{
    public void AddToBlacklist(string token, Guid userId, DateTime timeOfRemoval);
    public void RemoveFromBlacklist(string token); 
    public Task<bool> IsTokenBlacklisted(string token); 
}