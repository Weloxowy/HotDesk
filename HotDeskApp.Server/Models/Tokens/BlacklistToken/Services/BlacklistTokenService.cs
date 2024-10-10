using HotDeskApp.Server.Models.Tokens.BlacklistToken.Repositories;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Persistance.Tokens.BlacklistToken.Repositories;

namespace gatherly.server.Persistence.Tokens.BlacklistToken;

public class BlacklistTokenService : IBlacklistTokenService
{
    private readonly IBlacklistTokenRepository _blacklistTokenRepository ;

    public BlacklistTokenService(IBlacklistTokenRepository blacklistTokenRepository)
    {
        _blacklistTokenRepository = blacklistTokenRepository;
    }
    

    public void AddToBlacklist(string token, Guid userId, DateTime timeOfRemoval)
    {
        var refreshToken = new HotDeskApp.Server.Models.Tokens.BlacklistToken.Entities.BlacklistToken()
        {
            Token = token,
            UserId = userId,
            EndOfBlacklisting = timeOfRemoval.AddMinutes(30)
        };
        _blacklistTokenRepository.AddToBlacklist(refreshToken);
    }

    public void RemoveFromBlacklist(string token)
    {
        _blacklistTokenRepository.RemoveFromBlacklist(token);
    }

    public async Task<bool> IsTokenBlacklisted(string token)
    {
        return await _blacklistTokenRepository.IsTokenBlacklisted(token);
    }
}