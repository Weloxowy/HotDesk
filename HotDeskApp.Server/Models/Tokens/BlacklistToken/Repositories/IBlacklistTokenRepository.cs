namespace HotDeskApp.Server.Models.Tokens.BlacklistToken.Repositories;

public interface IBlacklistTokenRepository
{
    public void AddToBlacklist(Entities.BlacklistToken token); 
    public void RemoveFromBlacklist(string token);
    public Task<bool> IsTokenBlacklisted(string token);
}